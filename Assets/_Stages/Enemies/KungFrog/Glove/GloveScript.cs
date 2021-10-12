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
        exploded = false,
        facingRight;

    private void Start()
    {
        player = FindObjectOfType<PlayerStateAnimationSound>().transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GetComponent<GravityReverse>().ObjectIsAFly();
        FindObjectOfType<AudioManager>().PlaySound("shurikenSpawn");
    }

    private void Update()
    {
        facingRight = transform.localScale.x > 0F;
        if (rb) MoveGlove();
    }



    private void MoveGlove()
    {
        Vector3 toTarget = facingRight ? player.position - transform.position : transform.position - player.position;
        if (toTarget.magnitude > maxDist)
        {
            float angle = Vector3.Angle(transform.right, toTarget);
            transform.right = Vector3.RotateTowards(transform.right, toTarget, maxRadian, maxMagnitude);
        }
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
            StartCoroutine(FindObjectOfType<CameraShake>().Shake(.5f, .05f));
            animator.SetTrigger("explode");
            FindObjectOfType<AudioManager>().PlaySound("explosion");
            transform.localScale *= 2F;
            collision.otherCollider.isTrigger = true;
            Destroy(rb);
            Destroy(gameObject, 2F);
            exploded = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStateAnimationSound player = collision.GetComponentInParent<PlayerStateAnimationSound>();
            player.HandleBurnDamage();
        }
    }
}
