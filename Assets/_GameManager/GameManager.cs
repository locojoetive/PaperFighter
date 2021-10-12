using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

// This is a Singleton
public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public AudioManager audioManager;
    public static bool paused;

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
        Application.targetFrameRate = 60;

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
        FindObjectOfType<StageManager>().OnLevelFinishedLoading(scene);
        FindObjectOfType<AudioManager>().OnLevelFinishedLoading(scene);
        FindObjectOfType<PlayThemes>().OnLevelFinishedLoading();
        FindObjectOfType<EnableHUDOnStage>().OnLevelFinishedLoading();
        FindObjectOfType<FollowPlayer>().OnLevelFinishedLoading();
    }

}
