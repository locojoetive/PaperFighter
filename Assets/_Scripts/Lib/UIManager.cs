﻿using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pauseButton;
    public GameObject overlay;
    public GameObject pauseMenu;
    public GameObject resumeButton;
    public GameObject textBox;

    void Awake()
    {
        overlay.SetActive(false);
    }

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
        paused = true;
        if (!TouchBehaviour.active) 
            EventSystem.current.SetSelectedGameObject(resumeButton);
        overlay.SetActive(true);
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0F;
    }

    public void OnResume()
    {
        overlay.SetActive(false);
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        paused = false;
        Time.timeScale = 1F;
    }

    public void OnRetry()
    {
        OnResume();
        StageManager.ReloadScene();
    }

    public void OnExit()
    {
        OnResume();
        StageManager.ExitScene();
    }
}
