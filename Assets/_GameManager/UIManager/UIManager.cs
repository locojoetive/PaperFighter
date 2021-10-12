using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pauseButton;
    public GameObject pauseMenu;
    public GameObject resumeButton;

    private void Update()
    {
        if (InputManager.escape)
        {
            if (paused)
            {
                OnResume();
            }
            else
            {
                OnPause();
            }
        }
    }

    public void OnPause()
    {
        if (InputManager.active)
        {
            paused = true;
            pauseMenu.SetActive(true);
            pauseButton.SetActive(false);
            Time.timeScale = 0F;

            if (!InputManager.touchActive)
            {
                EventSystem.current.SetSelectedGameObject(resumeButton);
            }
        }
    }

    public void OnResume()
    {
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        paused = false;
        Time.timeScale = 1F;
    }

    public void OnRetry()
    {
        OnResume();
        StageManager.ReloadStage();
    }

    public void OnExit()
    {
        OnResume();
        StageManager.ExitStage();
    }
}
