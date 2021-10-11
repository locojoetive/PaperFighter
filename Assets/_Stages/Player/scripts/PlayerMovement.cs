using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerStateAnimationSound state;
    private Rigidbody2D rb;
    
    private float
        wallJumpFrame = 0.2f;
    private int
        jumpHeight = 10;
    
    private float fRememberJumpPressedTime = 0.1f;
    private float fRememberReplenishJumpsTime = 0.1f;
    private float fRememberJumpPressed;
    private float fRememberReplenishJumps;
    private bool bReplenishJumps = false;
    private bool bJumping = false;
    
    public float damping;
    [Range(0.0f, 1.0f)] public float walkDamping;
    [Range(0.0f, 1.0f)] public float shootDamping;
    [Range(0.0f, 1.0f)] public float shootDampingMidAir;
    [Range(0.0f, 1.0f)] public float stopDamping;
    public float walkingAcceleration;
    public float maxVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        state = GetComponent<PlayerStateAnimationSound>();
    }

    private void Update()
    {
        if (state.wallGrabbing && state.move)
        {
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.isKinematic = false;
        }
        if (state.move)
        {
            HandleMovement();
        } else if (!state.shurikenThrown)
        {
            float horizontalVelocity = rb.velocity.x;
            if (state.grounded)
                horizontalVelocity *= Mathf.Pow(1f - shootDamping, Time.deltaTime * damping);
            else
                horizontalVelocity *= Mathf.Pow(1f - shootDampingMidAir, Time.deltaTime * damping);
            horizontalVelocity = Mathf.Clamp(horizontalVelocity, -maxVelocity, maxVelocity);
            if (!float.IsNaN(horizontalVelocity))
                rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
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
        if (!rb.isKinematic)
        {
            float horizontalVelocity = rb.velocity.x;
            float xAxis = InputManager.xAxis;
            horizontalVelocity += xAxis * walkingAcceleration / Time.deltaTime;
            if (Mathf.Abs(xAxis) < 0.01f)
                horizontalVelocity *= Mathf.Pow(1f - stopDamping, Time.deltaTime * damping);
            else
                horizontalVelocity *= Mathf.Pow(1f - walkDamping, Time.deltaTime * damping);

            horizontalVelocity = Mathf.Clamp(horizontalVelocity, -maxVelocity, maxVelocity);
            if (!float.IsNaN(horizontalVelocity))
                rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
        }
        if (!state.hit && state.recovered)
        {
            HandleJump();
        }
    }

    public void HandleJump()
    {
        bool actuallyGrounded = state.grounded && ((!state.upsideDown && rb.velocity.y < 0) || (state.upsideDown && rb.velocity.y > 0));

        bReplenishJumps = fRememberReplenishJumps > 0f;
        bJumping = fRememberJumpPressed > 0f;

        fRememberReplenishJumps -= Time.deltaTime;
        if (actuallyGrounded || state.wallGrabbing)
        {
            fRememberReplenishJumps = fRememberReplenishJumpsTime;
        }

        fRememberJumpPressed -= Time.deltaTime;    
        if (InputManager.jump)
        {
            fRememberJumpPressed = fRememberJumpPressedTime;
        }

        if (bReplenishJumps)
        {
            if (bJumping)
            {
                fRememberJumpPressed = 0f;
                fRememberReplenishJumps = 0f;

                state.jumpNo = 1;
                state.PlayJumpSound();
                Jump();
            } else if (actuallyGrounded)
            {
                state.jumpNo = 0;
            }
        } else if (bJumping && state.jumpNo < 2)
        {
            fRememberJumpPressed = 0f;

            state.jumpNo = 2;
            state.PlayJumpSound();
            Jump();
        }
        else if (state.jumpNo == 0)
        {
            state.jumpNo = 1;
        } 
    }

    private void Jump()
    {
        if (state.wallGrabbing)
        {
            if (state.facingRight)
            {
                state.Flip(-1);
                rb.velocity = new Vector2(-5, jumpHeight);
            }
            else
            {
                state.Flip(1);
                rb.velocity = new Vector2(5, jumpHeight);
            }
            state.FreezeMoves(wallJumpFrame);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            Debug.Log("JUMP NO." + state.jumpNo);
            fRememberReplenishJumps = 0f;
        }
    }
}