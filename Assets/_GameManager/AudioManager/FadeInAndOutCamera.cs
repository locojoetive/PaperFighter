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

    private System.Collections.IEnumerator FadeOut(string sceneName)
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

    public void OnLevelFinishedLoading()
    {
        animator.SetBool("Fade", false);
    }
}
