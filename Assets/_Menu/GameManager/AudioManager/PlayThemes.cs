using UnityEngine;

enum SceneType
{
    INTRO,
    STAGE_1,
    STAGE_2,
    BEFORE_BOSS,
    IN_BOSS,
    AFTER_BOSS,
    OUTRO
}

public class PlayThemes : MonoBehaviour
{
    AudioManager audioManager;
    KungFrogStatesAnimationSound kungFrog;
    SceneType sceneType;
    bool victory = false;

    void Awake()
    {
        audioManager = GetComponent<AudioManager>();
    }

    void Update()
    {
        if (kungFrog != null)
        {
            if (kungFrog.fightStarted && kungFrog.life > 0 && sceneType != SceneType.IN_BOSS)
            {
                sceneType = SceneType.IN_BOSS;
                HandleTheme();
            }
            else if (kungFrog.life == 0 && sceneType != SceneType.AFTER_BOSS)
            {
                sceneType = SceneType.AFTER_BOSS;
                HandleTheme();
            }
        }
    }

    private void HandleTheme()
    {
        switch (sceneType)
        {
            case SceneType.INTRO:
                audioManager.PlayTheme("intro");
                break;
            case SceneType.STAGE_1:
                audioManager.PlayTheme("stage_1");
                break;
            case SceneType.STAGE_2:
                audioManager.PlayTheme("stage_2");
                break;
            case SceneType.BEFORE_BOSS:
                audioManager.PlayTheme("before_boss");
                break;
            case SceneType.IN_BOSS:
                audioManager.PlayTheme("in_boss");
                break;
            case SceneType.AFTER_BOSS:
                if (!victory)
                {
                    audioManager.FadeOutOtherThemes();
                    audioManager.PlaySound("victory");
                    victory = true;
                }
                break;
            case SceneType.OUTRO:
                audioManager.PlayTheme("outro");
                break;
            default:
                break;
        }
    }

    public void OnLevelFinishedLoading()
    {
        kungFrog = FindObjectOfType<KungFrogStatesAnimationSound>();
        if (StageManager.onTitle || StageManager.onControls|| StageManager.onIntro)
        {
            sceneType = SceneType.INTRO;
        }
        else if (kungFrog != null)
        {
            sceneType = SceneType.BEFORE_BOSS;
        }
        else if (StageManager.onStage)
        {
            if (StageManager.currentStage == 1 || StageManager.currentStage == 2)
            {
                sceneType = SceneType.STAGE_1;
            }
            if (StageManager.currentStage == 3 || StageManager.currentStage == 4)
            {
                sceneType = SceneType.STAGE_2;
            }
        }
        else if (StageManager.onOutro || StageManager.onCredits)
        {
            sceneType = SceneType.OUTRO;
        }

        HandleTheme();
    }
}
