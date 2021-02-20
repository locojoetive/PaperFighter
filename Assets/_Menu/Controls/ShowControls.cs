using UnityEngine;

public class ShowControls : MonoBehaviour
{

    void Start()
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        Transform touch = System.Array.Find(children, child => child.name == "Touch");
        Transform pc = System.Array.Find(children, child => child.name == "PC");

        touch.gameObject.SetActive(InputManager.touchActive);
        pc.gameObject.SetActive(!InputManager.touchActive);
    }
}
