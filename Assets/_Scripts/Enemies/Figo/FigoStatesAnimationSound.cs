using UnityEngine;
using System.Collections;
using System;

public class FigoStatesAnimationSound : MonoBehaviour {
    private AudioSource source;
    private Transform player;
    public AudioClip fire;
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
        source = GetComponent<AudioSource>();
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
        attacking = true;
    }

    internal void DestroyFigo()
    {
        Destroy(gameObject);
    }
}
