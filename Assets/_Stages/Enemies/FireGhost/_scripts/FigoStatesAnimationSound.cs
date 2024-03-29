﻿using UnityEngine;

public class FigoStatesAnimationSound : MonoBehaviour {
    private Transform player;
    public Transform playerDetector;
    private Animator animator;
    public bool
        detected = false,
        attacking = false,
        dead = false;

    public float detectionRadius = 5.0f;
    public LayerMask whatIsPlayer;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }
	
	void Update () {
        detected = Physics2D.OverlapCircle(playerDetector.position, detectionRadius, whatIsPlayer);
        HandleAnimation();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "weapon")
        {
            FindObjectOfType<AudioManager>().PlaySound("hitEnemy");
            dead = true;
        }
    }

    private void HandleAnimation()
    {
        if (attacking)
        {
            animator.SetTrigger("Attack");
            attacking = false;
        }
        if (dead)
        {
            Destroy(GetComponent<GravityReverse>());
            Destroy(GetComponent<Rigidbody2D>());
            animator.SetTrigger("Dead");
            dead = false;
        }
    }

    internal bool playerDetected()
    {
        return detected;
    }

    internal Vector2 relativePlayerPosition()
    {
        return (Vector2)player.position - (Vector2)transform.position;
    }

    internal void triggerAttack()
    {
        FindObjectOfType<AudioManager>().PlaySound("fire", .5f);
        attacking = true;
    }

    internal void DestroyFigo()
    {
        FindObjectOfType<AudioManager>().StopSound("fire");
        Destroy(gameObject);
    }
}
