using System.Collections;
using UnityEngine;

public class PlayerStateAnimationSound : MonoBehaviour {

    private Rigidbody2D rb;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public AudioManager audioManager;
    internal Collider2D groundedOn = null;

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
        wallGrabbing,
        invincible;
    private bool crit;
    public float
        freezeUntil = 0.0f,
        groundRadius = 0.2f,
        horizontalSpeed,
        verticalVelocity,
        wallRadius = 0.2f;

    //Damage stuff
    private HeartScript heart;
    private int LifeCount = 3;
    private float 
        recovery = 0.0f,
        recoverAfter = 2F;
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
        grounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(1f , groundRadius), 0f, whatIsGround);
        groundedOn = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
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


        wallGrabbing = !grounded &&
            onWall &&
            commitToDirection;
        move = shurikenThrown && freezeUntil < Time.time && recovered && !hit;
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
        if (!invincible)
        {
            ResetReleaseWhenHit();
            if (recovered && !hit)
            {
                StartCoroutine(FindObjectOfType<CameraShake>().Shake(.5f, .05f));
                move = false;
                animator.SetTrigger("Burned");
                HandleDamage();
                audioManager.PlaySound("fireDamage");
            }
        }
    }

    public void Electrified()
    {
        if (!invincible)
        {
            ResetReleaseWhenHit();
            if (recovered && !hit)
            {
                StartCoroutine(FindObjectOfType<CameraShake>().Shake(.25f, .2f));
                move = false;
                animator.SetTrigger("Electrified");
                audioManager.PlaySound("lightningDamage");
                HandleDamage();
            }
        }
    }

    public void HandleNormalDamage()
    {
        if (!invincible)
        {
            ResetReleaseWhenHit();
            if (recovered && !hit)
            {
                StartCoroutine(FindObjectOfType<CameraShake>().Shake(.5f, .05f));
                move = false;
                animator.SetTrigger("Crit");
                HandleDamage();
                audioManager.PlaySound("shurikenSpawn");
            }
        }
    }
    public void HandleDamage()
    {
        GetComponent<PlayerAttack>().abortAttack();
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

    private void Recovery()
    {
        if (hit && grounded)
        {
            hit = false;
            recovered = false;
            StartCoroutine(FindObjectOfType<CameraShake>().Shake(.1f, .1f));
            StartCoroutine(Flash());
            recovery = Time.time + recoverAfter;
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
        StageManager.ReloadStage();
    }

    internal void setUpsideDown(bool _upsideDown)
    {
        upsideDown = _upsideDown;
    }

    internal void PlayJumpSound() {
        float rnd = Random.Range(0.1f, 0.15f);
        if (rnd < 0.5f)
            audioManager.PlaySound("jumpA");
        else
            audioManager.PlaySound("jumpB");
    }

    internal void FreezeMoves(float time)
    {
        move = false;
        freezeUntil = Time.time + time;
    }

    internal bool Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            return true;
        }
        return false;
    }

    public void ResetReleaseWhenHit()
    {
        shurikenThrown = true;
        animator.SetBool("Aiming", false);
        animator.SetBool("ReleaseAttack", false);
    }

    private IEnumerator Flash()
    {
        invincible = true;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color flashOn = Color.clear;
        Color flashOff = renderer.color;
        for (int i = 0; i < 20; i++)
        {
            renderer.color = flashOn;
            yield return new WaitForSeconds(.1f);
            renderer.color = flashOff;
            yield return new WaitForSeconds(.1f);
        }
        invincible = false;
    }
}
