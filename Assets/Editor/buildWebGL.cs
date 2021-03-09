/*
  This script adds a build menu "HTML5 Export" to a Unity project to ease exporting to WebGL with
  different options.
  To set up for a project:
   1. Copy this script under ProjectRoot/Assets/Editor/buildWebGL.cs, and
   2. edit the variable levelsToPack below to list all the scene files that need to be packed as
      part of the build. If there are multiple scenes, have the main/startup scene listed first.

  When one of the build entries in the HTML5 Export dropdown menu is activated, a WebGL build will
  be generated under a sibling directory of the project named
  html5_builds/<ProjectName>_<timestamp>_<options>/, and a build log file is generated in the
  outputted directory.
*/
using UnityEditor;
using UnityEngine;
using System;
using System.IO;

public class ExportWebGL : MonoBehaviour
{
    private static string[] levelsToPack = new string[] {
        "Assets/Scenes/pf_title.unity",
        "Assets/Scenes/pf_intro.unity",
        "Assets/Scenes/pf_stage_1.unity",
        "Assets/Scenes/pf_stage_2.unity",
        "Assets/Scenes/pf_stage_3.unity",
        "Assets/Scenes/pf_stage_4.unity",
        "Assets/Scenes/pf_stage_5.unity",
        "Assets/Scenes/pf_credits.unity",
        "Assets/Scenes/pf_outro.unity",
    };

    private static string buildLogFile = null;

    static void WriteLog(string s)
    {
        try
        {
            StreamWriter sw = new StreamWriter(buildLogFile, true);
            sw.WriteLine(s);
            sw.Close();
        }
        catch (Exception)
        {
        }
    }
    static string FormatBytes(ulong bytes)
    {
        double gb = bytes / (1024.0 * 1024.0 * 1024.0);
        double mb = bytes / (1024.0 * 1024.0);
        double kb = bytes / 1024.0;
        if (mb >= 1000) return gb.ToString("#.000") + "GB";
        if (kb >= 1000) return mb.ToString("#.000") + "MB";
        if (kb >= 1) return kb.ToString("#.000") + "KB";
        return bytes + "B";
    }

    static string TimeSpanToHHMMSSString(TimeSpan s)
    {
        return s.ToString();
        /*
        if (s.ToString("hh") != "00")
            return s.ToString("hh") + "h " + s.ToString("mm") + "m " + s.ToString("ss") + "s";
        else
            return s.ToString("mm") + "m " + s.ToString("ss") + "s";
            */
    }

    static string PathRelativeTo(string path, string basePathRelativeTo)
    {
        // Abuse URI computation to compute a path relative to another
        return new Uri(basePathRelativeTo + "/").MakeRelativeUri(new Uri(path)).ToString();
    }

    [MenuItem("MyTools/WebGL Toll")]
    static void DoHtml5BuildToDirectory(string path, string emscriptenLinkerFlags, WebGLCompressionFormat compressionFormat, bool wasm)
    {
        PlayerSettings.WebGL.linkerTarget = wasm ? WebGLLinkerTarget.Wasm : WebGLLinkerTarget.Asm;
        PlayerSettings.WebGL.threadsSupport = false;
        PlayerSettings.WebGL.memorySize = 256;
        PlayerSettings.WebGL.emscriptenArgs = " -s TOTAL_STACK=1MB " + " -s ERROR_ON_UNDEFINED_SYMBOLS=0 " + emscriptenLinkerFlags;
        PlayerSettings.WebGL.compressionFormat = compressionFormat;
        PlayerSettings.WebGL.decompressionFallback = true;
        PlayerSettings.WebGL.dataCaching = true;
        PlayerSettings.WebGL.wasmArithmeticExceptions = WebGLWasmArithmeticExceptions.Ignore;

        PlayerSettings.defaultScreenWidth = 960;
        PlayerSettings.defaultScreenWidth = 540;

        Debug.Log("Starting a HTML5 build with Emscripten linker flags \"" + PlayerSettings.WebGL.emscriptenArgs + "\" to directory \"" + path + "\"...");

        if (!System.IO.Directory.Exists(path))
            System.IO.Directory.CreateDirectory(path);
        buildLogFile = path + "/build_log.txt";

        WriteLog("Unity version: " + Application.unityVersion);
        WriteLog("Project: " + Application.companyName + " " + Application.productName + " " + Application.version);
        WriteLog("Build date: " + DateTime.Now.ToString("yyyy MMM dd HH:mm:ss"));
        WriteLog("");
        var buildStart = DateTime.Now;
        UnityEditor.Build.Reporting.BuildReport report = BuildPipeline.BuildPlayer(levelsToPack, path, BuildTarget.WebGL, path.Contains("development") ? BuildOptions.Development : BuildOptions.None);
        var buildEnd = DateTime.Now;
        Debug.Log("HTML5 build finished in " + TimeSpanToHHMMSSString(buildEnd.Subtract(buildStart)) + " to directory " + path);
        WriteLog("HTML5 build finished in " + TimeSpanToHHMMSSString(buildEnd.Subtract(buildStart)));
        WriteLog("");
        ulong totalSize = 0;
        foreach (var f in report.files)
        {
            string relativePath = PathRelativeTo(f.path, path);
            if (relativePath.StartsWith(".."))
                continue; // report.files contains paths that are not part of the HTML5/WebGL build output (such as "Temp/StagingArea/Data/Managed/System.Xml.Linq.dll"), so ignore all those
            Debug.Log(relativePath + ": " + FormatBytes(f.size));
            WriteLog(relativePath + ": " + FormatBytes(f.size));
            totalSize += f.size;
        }
        Debug.Log("Total output size (Compression " + compressionFormat.ToString() + "): " + FormatBytes(totalSize));
        WriteLog("");
        WriteLog("Total output size (Compression " + compressionFormat.ToString() + "): " + FormatBytes(totalSize));
    }

    static void DoHtml5Build(string kind, string emscriptenLinkerFlags, WebGLCompressionFormat compressionFormat, bool wasm)
    {
        var date = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var path = System.IO.Path.GetFullPath(Application.dataPath + "/../../html5_builds/" + Application.productName + "_" + date + "_" + kind);
        DoHtml5BuildToDirectory(path, emscriptenLinkerFlags, compressionFormat, wasm);
    }

    static void DoHtml5BuildAskDirectory(string emscriptenLinkerFlags, WebGLCompressionFormat compressionFormat, bool wasm)
    {
        var path = EditorUtility.SaveFolderPanel("Choose Output Location for HTML5 Export", "", "");
        DoHtml5BuildToDirectory(path, emscriptenLinkerFlags, compressionFormat, wasm);
    }

    [MenuItem("HTML5 Export/Asm.js+Development uncompressed...")]
    static void DoAsmDevelopmentExportUncompressed()
    {
        DoHtml5Build("asm_development_uncompressed", "", WebGLCompressionFormat.Disabled, false);
    }

    [MenuItem("HTML5 Export/Wasm+Development uncompressed...")]
    static void DoDevelopmentExportUncompressed()
    {
        DoHtml5Build("wasm_development_uncompressed", "", WebGLCompressionFormat.Disabled, true);
    }

    [MenuItem("HTML5 Export/Wasm+Release gzipped...")]
    static void DoReleaseExportGzipped()
    {
        DoHtml5Build("wasm_release_gzipped", "", WebGLCompressionFormat.Gzip, true);
    }

    [MenuItem("HTML5 Export/Wasm+Release uncompressed...")]
    static void DoReleaseExportUncompressed()
    {
        DoHtml5Build("wasm_release", "", WebGLCompressionFormat.Disabled, true);
    }

    [MenuItem("HTML5 Export/Wasm+Release+Profiling uncompressed...")]
    static void DoProfilingExport()
    {
        DoHtml5Build("wasm_release_profiling", "--profiling-funcs", WebGLCompressionFormat.Disabled, true);
    }

    [MenuItem("HTML5 Export/Wasm+Release+CpuProfiler uncompressed...")]
    static void DoCpuProfilerExport()
    {
        DoHtml5Build("wasm_release_cpuprofiler", "--profiling-funcs --cpuprofiler", WebGLCompressionFormat.Disabled, true);
    }

    [MenuItem("HTML5 Export/Wasm+Release+MemoryProfiler uncompressed...")]
    static void DoMemoryProfilerExport()
    {
        DoHtml5Build("wasm_release_memoryprofiler", "--profiling-funcs --memoryprofiler", WebGLCompressionFormat.Disabled, true);
    }
}
