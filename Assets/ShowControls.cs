using System;
using UnityEngine;

public class ShowControls : MonoBehaviour
{

    void Start()
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        Transform touch = Array.Find(children, child => child.name == "Touch");
        Transform pc = Array.Find(children, child => child.name == "PC");

        touch.gameObject.SetActive(InputManager.touchActive);
        pc.gameObject.SetActive(!InputManager.touchActive);
    }
}
