using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static bool
        active = false,
        action,
        actionContinuous,
        actionRelease,
        confirm,
        down,
        escape,
        hold,
        jump,
        jumpContinuous,
        jumpRelease,
        left,
        right,
        reset,
        up;
    public static float
        xAxis,
        yAxis;
    TouchField touch;
    FloatingJoystick joystick;
    public static bool touchActive;

    void Start()
    {
        touch = FindObjectOfType<TouchField>(true);
        joystick = FindObjectOfType<FloatingJoystick>(true);

    }

    private void Update()
    {
        if (active)
        {
            HandleInputs();
        } 
        else
        {
            ResetKeys();
        }
    }

    private void HandleInputs()
    {
        // Decide which input device to listen
        if (Input.touches.Length > 0)
        {
            touchActive = true;
        }

        KeyBoardHandler.active = !touchActive;

        // Listen to active input device
        if (touchActive)
        {
            HandleTouchInputs();
        }
        else if (KeyBoardHandler.active)
        {
            HandleKeyBoardInputs();
        }
    }

    private void ResetKeys()
    {
        touchActive = false;
        KeyBoardHandler.active = false;

        xAxis = 0F;
        yAxis = 0F;
        left = false;
        up = false;
        down = false;
        right = false;
        action = false;
        actionContinuous = false;
        actionRelease = false;
        jump = false;
        jumpContinuous = false;
        jumpRelease = false;
        escape = false;
        confirm = false;
        reset = false;
    }

    private void HandleKeyBoardInputs()
    {
        xAxis = KeyBoardHandler.xAxis;
        yAxis = KeyBoardHandler.yAxis;
        left = KeyBoardHandler.left;
        up = KeyBoardHandler.up;
        down = KeyBoardHandler.down;
        right = KeyBoardHandler.right;
        action = KeyBoardHandler.action;
        actionContinuous = KeyBoardHandler.actionContinuous;
        actionRelease = KeyBoardHandler.actionRelease;
        jump = KeyBoardHandler.jump;
        jumpContinuous = KeyBoardHandler.jumpContinuous;
        jumpRelease = KeyBoardHandler.jumpRelease;
        escape = KeyBoardHandler.escape;
        confirm = KeyBoardHandler.confirm;
        reset = KeyBoardHandler.reset;
    }


    private void HandleTouchInputs()
    {
        jump = touch.jump;
        jumpContinuous = touch.jumpContinuous;
        jumpRelease = touch.jumpRelease;
        action = touch.swipe;
        actionContinuous = touch.swipeContinuous;
        actionRelease = touch.swipeRelease;
        confirm = touch.confirm;

        xAxis = joystick.xAxis;
        // yAxis = joystick.Vertical;
        left = joystick.left;
        up = joystick.up;
        down = joystick.down;
        right = joystick.right;
    }
}
