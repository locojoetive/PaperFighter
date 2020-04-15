using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// This is a Singleton
public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public AudioManager audioManager;
    public static bool paused;

    private void HandleAudioThemeFor(Scene scene)
    {
        if (scene.name == "pf_stage_1" || scene.name == "pf_stage_2")
            audioManager.PlayTheme("theme1");
        else if (scene.name == "pf_stage_3" || scene.name == "pf_stage_4")
            audioManager.PlayTheme("theme2");
        else
            audioManager.StopTheme();
    }

    // The following Methods handle the event of a freshly loaded scene
    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        HandleAudioThemeFor(scene);

        StartCoroutine(ActivateControlsIn(1F));
    }

    private IEnumerator ActivateControlsIn(float v)
    {
        yield return new WaitForSeconds(v);
        InputManager.active = true;
    }


    // Singleton pattern
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
}
