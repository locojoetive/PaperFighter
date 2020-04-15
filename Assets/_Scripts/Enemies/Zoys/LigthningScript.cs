using UnityEngine;
using System.Collections;

public class LigthningScript : MonoBehaviour {

    private Animator animator;
    private AudioSource source;
    private Rigidbody2D rb;
    private GameObject player;
    private bool exploded = false;

    public AudioClip expl;
    public float speed;
    public Vector3 relativePos;
    private bool inRange = false;

    void Start () {
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        Destroy(gameObject, 5);
    }

    void Update()
    {
        if (!exploded)
        {
            relativePos = -transform.up;
            if (!inRange)
            {
                relativePos = player.transform.position - transform.position;
                transform.up = -relativePos;
                rb.velocity = -transform.up * speed;
                inRange = relativePos.magnitude < 5F;
            } else rb.velocity = -transform.up * speed;
        }
    }
        
    
	
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (exploded)
                other.gameObject.GetComponent<PlayerStateAnimationSound>().HandleBurnDamage();
            else
            {
                other.gameObject.GetComponent<PlayerStateAnimationSound>().Electrified();
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.tag != "gravityReverse") {
            animator.SetTrigger("Explode");
            source.PlayOneShot(expl, 1F);
            Destroy(rb);
            exploded = true;
        }
    }
    

    void Explode()
    {
        Destroy(gameObject);
    }
}
