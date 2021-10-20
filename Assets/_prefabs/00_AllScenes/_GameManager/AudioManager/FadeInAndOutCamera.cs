using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInAndOutCamera : MonoBehaviour
{
    private Animator animator;
    private AudioManager audioManager;
    private SpriteRenderer black;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        black = GetComponent<SpriteRenderer>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private System.Collections.IEnumerator FadeOut(string sceneName)
    {
        InputManager.active = false;
        animator.SetBool("Fade", true);
        GameManager.paused = true;
        Sound currentTheme = audioManager.currentTheme;
        bool playSameTheme = audioManager.PlaySameTheme(sceneName);
        yield return new WaitUntil(() => {
            return black.color.a == 1 &&  (playSameTheme || currentTheme == null || !currentTheme.source.isPlaying);
        });
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void FadeToNextScene(string sceneName)
    {
        audioManager.FadeOutTheme(sceneName);
        StartCoroutine(FadeOut(sceneName));
    }

    public void OnLevelFinishedLoading()
    {
        animator.SetBool("Fade", false);
        StartCoroutine(ActivateControlsAfterFadeIn());
    }


    private System.Collections.IEnumerator ActivateControlsAfterFadeIn()
    {
        yield return new WaitUntil(() => {
            return black.color.a == 0;
        });
        InputManager.active = true;
    }
}
