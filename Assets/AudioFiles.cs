using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFiles : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
