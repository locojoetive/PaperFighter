using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerStateAnimationSound state;
    private Rigidbody2D rb;
    
    private float
        wallJumpFrame = 0.2f;
    private int
        jumpHeight = 10;
    public int walkingSpeed = 8;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        state = GetComponent<PlayerStateAnimationSound>();
    }

    private void Update()
    {
        if (state.move)
        {
            HandleMovement();
        }
        HandleGravity();
    }


    private void HandleGravity()
    {
        bool upsideDown = state.upsideDown;
        jumpHeight = upsideDown ? - Mathf.Abs(jumpHeight) : Mathf.Abs(jumpHeight);
    }

    private void HandleMovement()
    {
        state.Flip(state.horizontalSpeed);
        if (state.wallGrabbing)
        {
            rb.velocity = Vector3.zero;
        }
        rb.velocity = new Vector2(state.horizontalSpeed * walkingSpeed, rb.velocity.y);
        if (!InputManager.touchActive) Jump();
    }

    public void Jump()
    {
        if (InputManager.jump && state.jumpNo < 2)
        {
            Debug.Log("OK!");
            if (!state.wallGrabbing)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            }
            else
            {
                if (state.facingRight)
                {
                    rb.velocity = new Vector2(-5, jumpHeight);
                    state.Flip(-1);
                }
                else
                {
                    rb.velocity = new Vector2(5, jumpHeight);
                    state.Flip(1);
                }
                state.FreezeMoves(wallJumpFrame);
            }
            state.PlayJumpSound();
            state.incrementJumpNo();
        }
    }
}