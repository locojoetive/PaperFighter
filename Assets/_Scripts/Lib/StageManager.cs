﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    private static bool initializing = false;
    public static bool 
        hasIntroEnded,
        paused = false,
        onTitle,
        onControls,
        onIntro,
        onStage,
        onCredits;

    public static int 
        currentStage = 0,
        lastStage = 4;

    public static string activeScene;
    
    private static FadeInAndOutCamera fade;


    private void Update()
    {
        ProtocolOnTitle();
        ProtocolOnControls();
        ProtocolOnIntro();
        ProtocolOnStage();
        ProtocolOnCredits();
    }


    private void ProtocolOnTitle()
    {
        if (onTitle)
        {
            if (InputManager.confirm)
            {
                InputManager.confirm = false;
                fade.FadeToNextScene("pf_controls");
            }
        }

    }

    private void ProtocolOnControls()
    {
        if (onControls)
        {
            if (InputManager.escape)
            {
                InputManager.escape = false;
                fade.FadeToNextScene("pf_title");
            }
            else if (InputManager.confirm)
            {
                InputManager.confirm = false;
                currentStage = 1;
                fade.FadeToNextScene("pf_stage_" + currentStage);
            }
        }
    }


    private void ProtocolOnIntro()
    {
        if (onIntro && hasIntroEnded && InputManager.confirm)
        {
            InputManager.confirm = false;
            hasIntroEnded = false;
            currentStage = 1;
            fade.FadeToNextScene("pf_stage_" + currentStage);
        }
    }

    private void ProtocolOnStage()
    {
        if (onStage)
        {
            if (InputManager.reset)
            {
                fade.FadeToNextScene("pf_stage_" + currentStage);
            }
        }
    }

    private void ProtocolOnCredits()
    {
        if (onCredits)
        {
            if (PlayCredits.creditsHaveEnded && (InputManager.escape || InputManager.confirm))
            {
                PlayCredits.creditsHaveEnded = false;
                InputManager.escape = false;
                InputManager.confirm = false;
                fade.FadeToNextScene("pf_title");
            }
        }
    }

    public void IdentifyScene(Scene scene)
    {
        activeScene = scene.name;
        onTitle = activeScene.Contains("title");
        onControls = activeScene.Contains("controls");
        onIntro = activeScene.Contains("intro");
        onStage = activeScene.Contains("stage");
        onCredits = activeScene.Contains("credits");
        if (onStage)
        {
            currentStage = ExtractStageNumber(activeScene);
        }
    }

    private int ExtractStageNumber(string activeScene)
    {
        string[] splitSceneName = activeScene.Split('_');
        if (splitSceneName.Length == 3)
            return Int32.Parse(splitSceneName[2]);
        return -1;
    }

    public static void InitializeNextStage()
    {
        if (!initializing)
        {
            initializing = true;
            currentStage++;
            if (currentStage > 0 && currentStage <= lastStage)
            {
                fade.FadeToNextScene("pf_stage_" + currentStage);
            }
            else
            {
                fade.FadeToNextScene("pf_credits");
            }
        }
    }

    public static void ReloadScene()
    {
        fade.FadeToNextScene("pf_stage_" + currentStage);
    }

    public static void ExitScene()
    {
        fade.FadeToNextScene("pf_title");
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
        initializing = false;
        IdentifyScene(scene);

        if (fade == null)
        {
            fade = FindObjectOfType<FadeInAndOutCamera>();
        }
    }
}
