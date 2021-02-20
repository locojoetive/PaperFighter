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

    public void PlaySound(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
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

    public void PlayTheme(string name)
    {
        Sound t = System.Array.Find(themes, theme => theme.name == name);
        if (t == null)
        {
            return;
        }
        else if (t.source.isPlaying)
        {
            return;
        }
        FadeOutOtherThemes();
        t.source.Play();
    }

    public void StopTheme()
    {
        Sound t = System.Array.Find(themes, theme => theme.source.isPlaying);
        if (t != null) 
            t.source.Stop();
    }

    public void FadeOutOtherThemes()
    {
        Sound currentTheme = System.Array.Find(themes, theme => theme.source.isPlaying);
        if (currentTheme == null)
        {
            return;
        }
        if (!currentTheme.isFadingOut)
        {
            StartCoroutine(currentTheme.FadeOut());
        }
    }
}
