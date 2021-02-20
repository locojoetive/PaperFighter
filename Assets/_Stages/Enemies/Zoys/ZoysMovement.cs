using UnityEngine;

public class ZoysMovement : MonoBehaviour {

    private Rigidbody2D rb;
    private Animator animator;
    private GravityReverse gravity;
    private Transform player;

    public Transform playerDetection;
    public LayerMask whatIsPlayer;

    private Vector2 origin;
    private Vector3 velocity;
    private float
        abovePlayerFactor = 4F,
        freezeFor = 3F,
        radius = 15F,
        smoothTime = 0.5f,
        unfreezeAt = 0.0f;
    private bool playerDetected = false;
    public int
        life = 1;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gravity = GetComponent<GravityReverse>();
        gravity.ObjectIsAFly();
        origin = transform.position;
    }

    void Update () {
        HandleState();
        if (rb) HandleMovement();
	}

    private void HandleState()
    {
        playerDetected = Physics2D.OverlapCircle(playerDetection.position, radius, whatIsPlayer);
    }

    void HandleMovement()
    {
        if (isFrozen())
            rb.velocity = Vector2.zero;
        else if (playerDetected)
        {
            Vector3 abovePlayer = player.position + (gravity.isUpsidedown() ? -Vector3.up : Vector3.up ) * abovePlayerFactor;
            transform.position = Vector3.SmoothDamp(
                transform.position,
                abovePlayer,
                ref velocity,
                smoothTime
            );
        }
        else
        {
            transform.Translate(new Vector3(origin.x - transform.position.x, origin.y - transform.position.y, 0F));
        }
    }

    public bool isPlayerDetected()
    {
        return playerDetected;
    }

    public void freeze()
    {
        unfreezeAt = Time.time + freezeFor;
    }

    public bool isFrozen()
    {
        return unfreezeAt > Time.time;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "weapon")
        {
            life--;
            FindObjectOfType<AudioManager>().PlaySound("hitEnemy");
            if (life == 0)
            {
                animator.SetTrigger("Dead");
                Destroy(rb);
                GetComponent<BoxCollider2D>().isTrigger = true;
                Destroy(gameObject, 2);
            } else
            {
                animator.SetTrigger("Hit");
                freeze();
            }
        }
    }
}
