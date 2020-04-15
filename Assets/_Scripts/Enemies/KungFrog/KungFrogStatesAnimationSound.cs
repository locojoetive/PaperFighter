using System;
using System.Collections;
using System.Collections.Generic;
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
        tooClose;
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

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleStates();
        HandleAnimation();
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
            FindObjectOfType<AudioManager>().PlayTheme("theme3");
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
        if (!grounded && other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerStateAnimationSound>().HandleNormalDamage();
        }

        if (other.gameObject.tag == "weapon" && !other.gameObject.GetComponent<Collider2D>().isTrigger)
        {
            if (fightStarted)
            {
                life--;
                if (life == 0)
                {
                    animator.SetTrigger("dead");
                    dead = true;
                    GetComponent<BoxCollider2D>().isTrigger = true;
                    Destroy(rb);
                    Destroy(gameObject, 2);
                }
            }
            else
            {
                fightStarted = true;
                FindObjectOfType<AudioManager>().PlayTheme("theme3");
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
}
