using UnityEngine;

public class JumpButton : MonoBehaviour
{
    [SerializeField]
    private bool jump;

    [SerializeField]
    private bool jumpContinuous;

    [SerializeField]
    private bool jumpRelease;

    private bool waitedOneFrame = false;

    public bool Jump { get { return jump; } }
    public bool JumpContinuous { get { return jumpContinuous; } }
    public bool JumpRelease { get { return jumpRelease; } }

    private void Update()
    {
        if (jump)
        {
            if (waitedOneFrame)
            {
                jump = false;
                jumpContinuous = true;
            } else
            {
                waitedOneFrame = true;
            }
        }
        else
        {
            waitedOneFrame = false;
        }
    }
    /*
    public void OnJump()
    {
        bool jumping = jump || jumpContinuous || jumpRelease;
        if (!jumping)
        {
            jump = true;
        }
        else if (jump)
        {
            jump = false;
            jumpContinuous = true;
        }
        else if (jumpContinuous)
        {
            jump = false;
            jumpContinuous = false;
            jumpRelease = true;
        }
        else if (jumpRelease)
        {
            jump = false;
            jumpContinuous = false;
            jumpRelease = false;
        }
    }
    */

    public void OnPointerDown()
    {
        jump = true;
        jumpContinuous = false;
        jumpRelease = false;
        Debug.Log("Its goin DOWN");
    }

    public void OnPointerUp()
    {
        jump = false;
        jumpContinuous = false;
        jumpRelease = true;
        Debug.Log("Its goin UP!");
    }
}
