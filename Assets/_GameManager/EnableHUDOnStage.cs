using UnityEngine;

public class EnableHUDOnStage : MonoBehaviour
{
    public GameObject[] onlyDisplayOnStage;

    private void OnlyDiplayOnStage()
    {
        bool onStage = StageManager.onStage;
        System.Array.ForEach<GameObject>(onlyDisplayOnStage, go => go.SetActive(onStage));
        if (onStage)
        {
            ResetHearts();
        }
    }

    private void ResetHearts()
    {
        GameObject heart = System.Array.Find<GameObject>(onlyDisplayOnStage, go => go.GetComponent<HeartScript>() != null);
        if (heart != null) heart.GetComponent<HeartScript>().ResetHearts();
    }

    public void OnLevelFinishedLoading()
    {
        OnlyDiplayOnStage();
    }
}
