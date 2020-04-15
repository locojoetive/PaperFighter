using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInAndOutCamera : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer black;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        black = GetComponent<SpriteRenderer>();
    }

    private IEnumerator FadeOut(string sceneName)
    {
        animator.SetBool("Fade", true);
        GameManager.paused = true;
        yield return new WaitUntil(() => black.color.a == 1);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void FadeToNextScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

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
        animator.SetBool("Fade", false);
    }
}
