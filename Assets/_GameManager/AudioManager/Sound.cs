using UnityEngine;

[System.Serializable]
public class Sound
{
    [HideInInspector]
    public AudioSource source;
    public AudioClip clip;

    public string name;

    [HideInInspector]
    public bool loop;
    [HideInInspector]
    public bool isTheme;

    public float pitch = 1F;
    public float volume = .5f;

    [HideInInspector]
    public bool isFadingOut = false;
    private float fadeOutTargetVolume = .05f;
    private float fadeOutSpeed = 1F;

    public System.Collections.IEnumerator FadeOut()
    {
        isFadingOut = true;
        yield return new WaitUntil(() => {
            source.volume -= fadeOutSpeed * Time.deltaTime;
            return source.volume < fadeOutTargetVolume;
        });
        source.Stop();
        source.volume = volume;
        isFadingOut = false;
    }
}