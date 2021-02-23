using UnityEngine;

public class AudioManager : MonoBehaviour {
    private Sound[] themes;
    private Sound[] sounds;
    private AudioFiles audioFiles;

    public void OnLevelFinishedLoading()
    {
        audioFiles = FindObjectOfType<AudioFiles>();
        themes = audioFiles.themes;
        sounds = audioFiles.sounds;
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
