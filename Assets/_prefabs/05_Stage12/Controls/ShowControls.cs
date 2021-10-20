using UnityEngine;


public class ShowControls : MonoBehaviour
{
    [SerializeField] private GameObject mobile;
    [SerializeField] private GameObject keyboard;

    void Start()
    {
        mobile.SetActive(InputManager.touchActive);
        keyboard.SetActive(!InputManager.touchActive);
    }
}
