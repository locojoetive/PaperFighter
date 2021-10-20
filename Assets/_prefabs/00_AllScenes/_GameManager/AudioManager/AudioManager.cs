using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {
    private Sound[] themes = new Sound[0];
    private Sound[] sounds = new Sound[0];
    private AudioFiles audioFiles;
    public string currentScene = "";
    public Sound currentTheme;

    void Awake()
    {
        audioFiles = FindObjectOfType<AudioFiles>();
    }

    public bool PlaySameTheme(string newScene)
    {
        return ((currentScene == "pf_title" || currentScene == "pf_controls" || currentScene == "pf_intro")
            && (newScene == "pf_title" || newScene == "pf_controls" || newScene == "pf_intro"))
            || ((currentScene == "pf_stage_1" || currentScene == "pf_stage_2") && (newScene == "pf_stage_1" || newScene == "pf_stage_2"))
            || ((currentScene == "pf_stage_3" || currentScene == "pf_stage_4") && (newScene == "pf_stage_3" || newScene == "pf_stage_4"))
            || (currentScene == "pf_outro" && newScene == "pf_credits");
    }

    public void OnLevelFinishedLoading(Scene scene)
    {
        // Shall we keep playing current theme ?
        bool sameStage = currentScene == scene.name;
        bool sameTheme = sameStage || PlaySameTheme(scene.name);

        if (!sameStage)
        {
            audioFiles = FindObjectOfType<AudioFiles>();
            ResetAudioSources(sameTheme);
        }
        currentScene = scene.name;
    }

    private void ResetAudioSources(bool sameTheme)
    {
        if (!sameTheme)
        {
            foreach (Sound theme in themes)
            {
                Destroy(theme.source);
            }
            themes = audioFiles.themes;
            foreach (Sound theme in themes)
            { 
                theme.source = gameObject.AddComponent<AudioSource>();
                theme.source.clip = theme.clip;
                theme.source.pitch = theme.pitch;
                theme.source.volume = theme.volume;
                theme.source.loop = true;
                theme.isTheme = true;
            }

        }
        foreach (Sound sound in sounds)
        {
            Destroy(sound.source);
        }
        sounds = audioFiles.sounds;
        foreach (Sound sound in sounds)
        {
            Destroy(sound.source);
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.pitch = sound.pitch;
            sound.source.volume = sound.volume;
            sound.source.loop = false;
            sound.isTheme = false;
        }
    }


    public void PlaySound(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " does not exist");
            return;
        }
        else if (s.source.isPlaying)
        {
            s.source.Stop();
        }
        s.source.pitch = s.pitch;
        s.source.volume = s.volume;
        s.source.Play();
    }

    public void PlaySound(string name, float pitch)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " does not exist");
            return;
        }
        else if (s.source.isPlaying)
        {
            s.source.Stop();
        }
        s.source.pitch = pitch;
        s.source.volume = s.volume;
        s.source.Play();
    }

    public void PlayTheme(string name)
    {
        Sound theme = Array.Find(themes, t => t.name == name);
        if (theme == null)
        {
            return;
        }
        else if (theme.source.isPlaying)
        {
            return;
        }
        theme.source.Play();
        currentTheme = theme;
    }

    public void StopTheme()
    {
        Sound theme = Array.Find(themes, t => t.name == name);
        if (theme != null)
            theme.source.Stop();
    }

    internal void StopSound(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound.source.isPlaying)
        {
            sound.source.Stop();
        }
    }

    public void FadeOutTheme(string newScene)
    {
        if (!PlaySameTheme(newScene))
        {
            Sound theme = currentTheme;
            if (theme == null)
            {
                return;
            }
            if (!theme.isFadingOut)
            {
                StartCoroutine(theme.FadeOut());
            }
        }
    }
}
