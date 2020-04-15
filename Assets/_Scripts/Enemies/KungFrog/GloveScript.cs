using System;
using UnityEngine;

public class GloveScript : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private Transform player;
    public float 
        maxDist,
        maxRadian,
        maxMagnitude,
        speed;
    private bool 
        goStraight = false,
        exploded = false,
        facingRight;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GetComponent<GravityReverse>().ObjectIsAFly();
        FindObjectOfType<AudioManager>().PlaySound("shurikenSpawn", 1F);
    }

    private void Update()
    {
        facingRight = transform.localScale.x > 0F;
        if (rb) MoveGlove();
    }



    private void MoveGlove()
    {
        Vector3 toTarget = facingRight ? player.position - transform.position : transform.position - player.position;
        if (!goStraight && toTarget.magnitude > maxDist)
        {
            float angle = Vector3.Angle(transform.right, toTarget);
            transform.right = Vector3.RotateTowards(transform.right, toTarget, maxRadian, maxMagnitude);
        }
        else goStraight = true;
        rb.velocity = facingRight ? speed * transform.right : -speed * transform.right;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerStateAnimationSound>().HandleNormalDamage();
        }

        if (!exploded)
        {
            animator.SetTrigger("explode");
            FindObjectOfType<AudioManager>().PlaySound("explosion", 1F);
            transform.localScale *= 2F;
            collision.otherCollider.isTrigger = true;
            Destroy(rb);
            Destroy(gameObject, 2F);
            exploded = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerStateAnimationSound>().HandleBurnDamage();
        }
    }
}
