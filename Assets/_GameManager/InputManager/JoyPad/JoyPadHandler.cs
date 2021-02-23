using UnityEngine;

public class JoyPadHandler : MonoBehaviour
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
    private static JoyPadHandler instance;
    internal static bool active;
    internal static bool actionRelease;

    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (active) HandleButtons();
    }


    private void HandleButtons()
    {
        xAxis = Mathf.Abs(Input.GetAxis("Horizontal")) < 0.35f ? 0F : Input.GetAxis("Horizontal");
        yAxis = Mathf.Abs(Input.GetAxis("Vertical")) < 0.35f ? 0F : Input.GetAxis("Vertical");
        left = xAxis < 0F;
        right = xAxis > 0F;
        down = yAxis < 0F;
        up = yAxis > 0F;
        action = Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1"); 
        actionContinuous = Input.GetButton("Jump") || Input.GetButton("Fire1");
        actionRelease = Input.GetButtonUp("Jump") || Input.GetButtonUp("Fire1");
        jump = Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3");
        jumpContinuous = Input.GetButton("Fire2") || Input.GetButton("Fire3");
        jumpRelease = Input.GetButtonUp("Fire2") || Input.GetButtonUp("Fire3");
        escape = Input.GetButtonDown("Escape");
        confirm = Input.GetButtonDown("Submit");
    }
    public static bool getActivity()
    {
        return Input.GetButton("Jump") 
            || Input.GetButton("Fire1") 
            || Input.GetButton("Fire2")
            || Input.GetButton("Fire3") 
            || Input.GetButton("Escape") 
            || Input.GetButton("Submit");
    }
}
