using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public Sound[] themes;
    public Sound[] sounds;
    private void Awake()
    {
        foreach (Sound theme in themes)
        {
            theme.source = gameObject.AddComponent<AudioSource>();
            theme.source.clip = theme.clip;
            theme.source.pitch = theme.pitch;
            theme.source.volume = theme.volume;
            theme.source.loop = true;
            theme.isTheme = true;
        }
        
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.pitch = sound.pitch;
            sound.source.volume = sound.volume;
            sound.source.loop = false;
            sound.isTheme = false;
        }

    }

    public void PlaySound(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Sound: " + name + " was not found");
            return;
        }
        else if (s.source.isPlaying)
        {
            Debug.Log("Sound: " + name + "is already Playing");
            s.source.Stop();
        }
        // s.source.pitch = pitch;
        s.source.Play();
    }

    public void PlayTheme(string name)
    {
        Sound t = Array.Find(themes, theme => theme.name == name);
        if (t == null)
        {
            Debug.LogError("Theme: " + name + " was not found");
            return;
        }
        else if (t.source.isPlaying)
        {
            Debug.LogWarning("Theme: " + name + "is already Playing");
            return;
        }
        FadeOutOtherThemes();
        t.source.Play();
    }

    public void StopTheme()
    {
        Sound t = Array.Find(themes, theme => theme.source.isPlaying);
        if (t != null) 
            t.source.Stop();
    }

    private void FadeOutOtherThemes()
    {
        Sound currentTheme = Array.Find(themes, theme => theme.source.isPlaying);
        if (currentTheme == null)
        {
            Debug.LogWarning("No theme was played before, therfore no fading out");
            return;
        }
        if (!currentTheme.isFadingOut)
        {
            StartCoroutine(currentTheme.FadeOut());
        }
    }
}
