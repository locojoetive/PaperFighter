using UnityEngine;

public class RenderImageOnCameraLayer : MonoBehaviour
{
    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
