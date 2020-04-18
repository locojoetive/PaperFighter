using UnityEngine;

public class PlayerStateAnimationSound : MonoBehaviour {

    private Rigidbody2D rb;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public AudioManager audioManager;

    public Transform groundCheck;
    public Transform wallDetect;
    public LayerMask whatIsGround;

    // States
    public bool jumping,
        commitToDirection,
        damageReceived = false,
        facingRight = true,
        grounded,
        hit = false,
        move = true,
        onWall,
        recovered = true,
        shurikenThrown = true,
        upsideDown = false,
        wallGrabbing;

    public float
        freezeUntil = 0.0f,
        groundRadius = 0.2f,
        horizontalSpeed,
        verticalVelocity,
        wallRadius = 0.2f;


    public int
        jumpNo = 0;

    //Damage stuff
    private HeartScript heart;
    private int LifeCount = 3;
    private float 
        recovery = 0.0f,
        recoveryTime = 2F;
    private float jumpButtonInactiveUntil = 0F;
    private bool jumpButton = false;
    private float jumpButtonInactiveFor = 0.07f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        heart = FindObjectOfType<HeartScript>();
        audioManager = FindObjectOfType<AudioManager>();
    }


    void Update() {
        Recovery();
        HandleAnimation();
        HandleStates();
    }

    private void HandleStates()
    {
        horizontalSpeed = InputManager.xAxis;
        commitToDirection = ((horizontalSpeed > 0 && facingRight) || (horizontalSpeed < 0 && !facingRight));
        verticalVelocity = rb.velocity.y;
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        onWall = Physics2D.OverlapCircle(wallDetect.position, wallRadius, whatIsGround);

        jumpButton = InputManager.jump && !jumping;
        if (jumpButton)
        {
            jumpButtonInactiveUntil = Time.time + jumpButtonInactiveFor;
            jumping = true;
        } else if (jumpButtonInactiveUntil < Time.time)
        {
            jumping = false;
        }

        jumpNo = !jumping 
            ? !grounded && !onWall && jumpNo == 0 
                ? 1 
                : grounded || onWall
                    ? 0
                    : jumpNo
            : jumpNo;

        wallGrabbing = !grounded &&
            onWall &&
            commitToDirection;
        move = shurikenThrown && freezeUntil < Time.time && recovered && (!hit && recovered);
    }


    private void HandleAnimation()
    {
        animator.SetBool("OnWall", wallGrabbing);
        animator.SetBool("Ground", grounded);
        animator.SetFloat("Speed", Mathf.Abs(horizontalSpeed));
        if (GetComponent<GravityReverse>().isUpsidedown())
        {
            animator.SetFloat("vSpeed", -verticalVelocity);
        }
        else
        {
            animator.SetFloat("vSpeed", verticalVelocity);
        }
    }

    public void HandleBurnDamage()
    {

        ResetReleaseWhenHit();
        if (recovered && !hit)
        {
            move = false;
            animator.SetTrigger("Burned");
            HandleDamage();
            audioManager.PlaySound("fireDamage", 1F);
        }
    }

    public void Electrified()
    {
        ResetReleaseWhenHit();
        if (recovered && !hit)
        {
            move = false;
            animator.SetTrigger("Electrified");
            audioManager.PlaySound("electricDamage", 1F);
            HandleDamage();
        }
    }

    public void HandleNormalDamage()
    {
        ResetReleaseWhenHit();
        if (recovered && !hit)
        {
            move = false;
            animator.SetTrigger("Crit");
            HandleDamage();
            audioManager.PlaySound("shurikenSpawn", 1F);
        }
    }
    public void HandleDamage()
    {
        rb.velocity = Vector2.zero;
        rb.velocity = new Vector3(facingRight ? -5 : 5, 2, 0);
        animator.SetBool("Hit", true);
        hit = true;
        move = false;
        if (LifeCount > 0)
        {
            heart.setHit();
            LifeCount--;
        }
        else
        {
            Die();
        }
    }

    public void Recovery()
    {
        if (hit && grounded)
        {
            hit = false;
            recovered = false;
            recovery = Time.time + recoveryTime;
        }
        if (!recovered && Time.time > recovery)
        {
            animator.SetBool("Hit", false);
            recovered = true;
            move = true;
        }
    }

    public void Die()
    {
        StageManager.ReloadScene();
    }

    internal void setUpsideDown(bool _upsideDown)
    {
        upsideDown = _upsideDown;
    }

    internal void incrementJumpNo()
    {
        jumpNo++;
    }

    internal void PlayJumpSound() {
        float rnd = Random.Range(0.5f, 0.75f);
        if (rnd < 0.625f)
            audioManager.PlaySound("jumpA", rnd);
        else
            audioManager.PlaySound("jumpB", rnd);
    }

    internal void FreezeMoves(float time)
    {
        move = false;
        freezeUntil = Time.time + time;
    }

    internal void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    public void ResetReleaseWhenHit()
    {
        shurikenThrown = true;
        animator.SetBool("Aiming", false);
        animator.SetBool("ReleaseAttack", false);
    }
}
