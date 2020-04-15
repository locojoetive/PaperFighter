using System;
using UnityEngine;

public class KeyBoardHandler : MonoBehaviour
{
    public static bool
        action,
        confirm,
        actionContinuous,
        down,
        escape,
        hold,
        jump,
        jumpContinuous,
        jumpRelease,
        left,
        right,
        reset,
        swipeUp,
        swipeDown,
        swipeLeft,
        swipeRight,
        up;
    public static float
        xAxis,
        yAxis;
    private new static GameObject gameObject;
    private static KeyBoardHandler instance;
    internal static bool active;
    internal static bool actionRelease;

    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        } else
        {
            instance = this;
        }
    }

    private void Update()
    {
        if(active) HandleKeys();
    }

    public static void HandleKeys()
    {
        up = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        down = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        action = Input.GetKeyDown(KeyCode.K)
            || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)
            || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
        actionContinuous = Input.GetKey(KeyCode.K)
            || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)
            || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        actionRelease = Input.GetKeyUp(KeyCode.K)
            || Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl)
            || Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift);
        jump = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.L);
        jumpContinuous = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.L);
        jumpRelease = Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.L);
        escape = Input.GetKeyDown(KeyCode.Escape);
        confirm = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return);
        reset = Input.GetKeyDown(KeyCode.R);
        xAxis = (right && left) || (!right && !left) 
            ? 0F 
            : right
                ? 1F 
                : -1F;
        yAxis = (up && down) || (!up && !down)
            ? 0F
            : up
                ? 1F
                : -1F;
    }
}