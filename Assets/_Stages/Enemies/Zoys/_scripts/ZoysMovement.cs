using System.Collections;
using UnityEngine;

public class ZoysMovement : MonoBehaviour {

    private Animator animator;
    private Transform player;

    public Transform playerDetection;
    public LayerMask whatIsPlayer;

    private Vector2 origin;
    private Vector3 velocity;
    private float
        abovePlayerFactor = 4F,
        freezeFor = 3F,
        radius = 15F,
        unfreezeAt = 0.0f;
    private bool playerDetected = false;
    private int life = 3;

    void Awake () {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerStateAnimationSound>().transform;
        origin = transform.position;
    }

    void Update () {
        HandleState();
        HandleMovement();
	}

    private void HandleState()
    {
        playerDetected = Physics2D.OverlapCircle(playerDetection.position, radius, whatIsPlayer);
    }

    void HandleMovement()
    {
        if (playerDetected)
        {
            Vector2 abovePlayer = player.position + Vector3.up * abovePlayerFactor;

            Vector3 move = ((Vector3) abovePlayer - transform.position) * Time.deltaTime;
            transform.position += move;
        }
        else
        {
            Vector3 moveBack = ((Vector3) origin - transform.position) * Time.deltaTime;
            transform.position += moveBack;
        }
        Debug.Log(transform.position);
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

    private IEnumerator Flash()
    {
        Physics2D.IgnoreLayerCollision(8, 9, true);
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        float flashAfter = 0.15f;
        Color flashOn = Color.clear;
        Color flashOff = renderer.color;
        for (int i = 0; i < 5; i++)
        {
            renderer.color = flashOn;
            yield return new WaitForSeconds(flashAfter);
            renderer.color = flashOff;
            yield return new WaitForSeconds(flashAfter);
        }
        Physics2D.IgnoreLayerCollision(8, 9, false);
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
                GetComponent<BoxCollider2D>().isTrigger = true;
                Destroy(gameObject, 2);
            } else
            {
                StartCoroutine(Flash());
            }
        }
    }
}
