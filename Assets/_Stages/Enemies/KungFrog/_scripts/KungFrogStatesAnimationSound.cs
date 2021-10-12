using System;
using System.Collections;
using UnityEngine;

public class KungFrogStatesAnimationSound : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;

    public Transform 
        groundCheckA,
        groundCheckB, 
        playerDetection,
        wallCheck;

    public LayerMask 
        whatIsGround,
        whatIsPlayer;

    public bool
        attacking = false,
        fightStarted = false,
        detected,
        tooClose,
        hasBeenAttacked;
    private bool 
        grounded = false,
        onWall = false,
        dead = false,
        walking;

    public int life = 1;

    public float 
        vertical = 0.0f, 
        detectionRadius,
        tooCloseRadius,
        wallRadius = 0.4f;

    public GameObject elevator;

    private new SpriteRenderer renderer;
    private Color flashOff;
    private bool hit;
    private bool wasGrounded;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        flashOff = renderer.color;
        Physics2D.IgnoreLayerCollision(8, 9, false);
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        if (elevator != null)
            elevator.SetActive(false);
    }

    private void Update()
    {
        HandleStates();
        HandleAnimation();
        HandleCameraShake();
    }

    private void HandleCameraShake()
    {
        if (!wasGrounded && grounded)
            StartCoroutine(FindObjectOfType<CameraShake>().Shake(.5f, .05f));
        wasGrounded = grounded;
    }

    private void HandleStates()
    {
        vertical = dead ? 0.0f : rb.velocity.y;
        grounded = Physics2D.OverlapCircle(groundCheckA.position, wallRadius, whatIsGround)
            && Physics2D.OverlapCircle(groundCheckB.position, wallRadius, whatIsGround);
        onWall = Physics2D.OverlapCircle(wallCheck.position, wallRadius, whatIsGround);
        detected = Physics2D.OverlapCircle(playerDetection.position, detectionRadius, whatIsPlayer);
        tooClose = Physics2D.OverlapCircle(playerDetection.position, tooCloseRadius, whatIsPlayer);
        walking = !detected && grounded;
        if (tooClose && !fightStarted)
        {
            fightStarted = true;
        }
    }


    private void HandleAnimation()
    {
        animator.SetFloat("vSpeed", vertical);
        animator.SetBool("grounded", grounded);
        animator.SetBool("walking", walking);
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && life > 0)
        {
            other.gameObject.GetComponent<PlayerStateAnimationSound>().HandleNormalDamage();
        }

        if (other.gameObject.tag == "weapon" && !other.gameObject.GetComponent<Collider2D>().isTrigger)
        {
            hasBeenAttacked = true;
            if (fightStarted)
            {
                life--;
                FindObjectOfType<AudioManager>().PlaySound("hitEnemy");
                if (life == 0)
                {
                    animator.SetTrigger("dead");
                    dead = true;
                    GetComponent<BoxCollider2D>().isTrigger = true;
                    Destroy(rb);
                    Destroy(gameObject, 2);
                    if (elevator != null) 
                        elevator.SetActive(true);
                }
                else
                {
                    StartCoroutine(Flash());
                }
            }
            else
            {
                fightStarted = true;
            }
        }
    }

    internal void initialzeAttack()
    {
        animator.ResetTrigger("jump");
        animator.SetTrigger("attack");
    }
    internal void initializeJump()
    {
        animator.SetTrigger("jump");
    }

    internal bool playerDetected()
    {
        return detected;
    }

    internal bool isPlayerTooClose()
    {
        return tooClose;
    }

    internal bool isGrounded()
    {
        return grounded;
    }
    internal bool isDead()
    {
        return dead;
    }

    internal bool isOnWall()
    {
        return onWall;
    }

    internal bool isFightStarted()
    {
        return fightStarted;
    }

    internal Vector2 getPlayerPosition()
    {
        return player.position;
    }


    private IEnumerator Flash()
    {
        Physics2D.IgnoreLayerCollision(8, 9, true);
        hit = true;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        float flashAfter = 0.15f;
        Color flashOn = Color.clear;
        for (int i = 0; i < 5; i++)
        {
            renderer.color = flashOn;
            yield return new WaitForSeconds(flashAfter);
            renderer.color = flashOff;
            yield return new WaitForSeconds(flashAfter);
        }
        hit = false;
        Physics2D.IgnoreLayerCollision(8, 9, false);
    }
}
