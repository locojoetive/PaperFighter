using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    private static bool initializing = false;
    public static bool 
        hasTextSceneEnded,
        paused = false,
        onTitle,
        onControls,
        onIntro,
        onStage,
        onOutro,
        onCredits;

    public static int 
        currentStage = 0,
        lastStage = 5;

    public static string activeScene;    
    private static FadeInAndOutCamera fade;

    private void Awake()
    {
        fade = FindObjectOfType<FadeInAndOutCamera>();
    }

    private void Update()
    {
        ProtocolOnTitle();
        ProtocolOnControls();
        ProtocolOnIntro();
        ProtocolOnStage();
        ProtocolOnOutro();
        ProtocolOnCredits();
    }


    private void ProtocolOnTitle()
    {
        if (onTitle)
        {
            if (InputManager.confirm)
            {
                InputManager.confirm = false;
                fade.FadeToNextScene("pf_controls");
            }
        }

    }

    private void ProtocolOnControls()
    {
        if (onControls)
        {
            if (InputManager.escape)
            {
                InputManager.escape = false;
                fade.FadeToNextScene("pf_title");
            }
            else if (InputManager.confirm)
            {
                InputManager.confirm = false;
                fade.FadeToNextScene("pf_intro");
            }
        }
    }


    private void ProtocolOnIntro()
    {
        if (!initializing && onIntro && hasTextSceneEnded)
        {
            initializing = true;
            currentStage = 1;
            fade.FadeToNextScene("pf_stage_" + currentStage);
        }
    }

    private void ProtocolOnStage()
    {
        if (onStage)
        {
            if (InputManager.reset)
            {
                fade.FadeToNextScene("pf_stage_" + currentStage);
            }
        }
    }
    private void ProtocolOnOutro()
    {
        if (onOutro && hasTextSceneEnded && InputManager.confirm)
        {
            InputManager.confirm = false;
            hasTextSceneEnded = false;
            fade.FadeToNextScene("pf_credits");
        }
    }

    private void ProtocolOnCredits()
    {
        if (onCredits)
        {
            if (!initializing && PlayCredits.creditsHaveEnded && (InputManager.escape || InputManager.confirm))
            {
                initializing = true;
                PlayCredits.creditsHaveEnded = false;
                InputManager.escape = false;
                InputManager.confirm = false;
                fade.FadeToNextScene("pf_title");
            }
        }
    }

    public void IdentifyScene(Scene scene)
    {
        activeScene = scene.name;
        onTitle = activeScene.Contains("title");
        onControls = activeScene.Contains("controls");
        onIntro = activeScene.Contains("intro");
        onStage = activeScene.Contains("stage");
        onOutro = activeScene.Contains("outro");
        onCredits = activeScene.Contains("credits");
        if (onStage)
        {
            currentStage = ExtractStageNumber(activeScene);
        }
    }

    private int ExtractStageNumber(string activeScene)
    {
        string[] splitSceneName = activeScene.Split('_');
        if (splitSceneName.Length == 3)
            return Int32.Parse(splitSceneName[2]);
        return -1;
    }

    public static void InitializeNextStage()
    {
        if (!initializing)
        {
            initializing = true;
            currentStage++;
            if (currentStage > 0 && currentStage <= lastStage)
            {
                fade.FadeToNextScene("pf_stage_" + currentStage);
            }
            else
            {
                fade.FadeToNextScene("pf_outro");
            }
        }
    }

    public static void ReloadStage()
    {
        fade.FadeToNextScene("pf_stage_" + currentStage);
    }

    public static void ExitStage()
    {
        fade.FadeToNextScene("pf_title");
    }

    public void OnLevelFinishedLoading(Scene scene)
    {
        hasTextSceneEnded = false;
        initializing = false;
        IdentifyScene(scene);
        fade.OnLevelFinishedLoading();
    }
}
