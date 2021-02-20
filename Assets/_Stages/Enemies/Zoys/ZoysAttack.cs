using UnityEngine;

public class ZoysAttack : MonoBehaviour {
    private Rigidbody2D rb;
    private Animator animator;
    private ZoysMovement movement;
    public Transform lightningSpawn;
    public GameObject lightning;

    private float 
        attackAt = 0.0f,
        coolDown = 5.0f;
    
	void Start () {
        movement = GetComponent<ZoysMovement>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
        HandleAttack();
        HandleGravity();
    }

    private void HandleGravity()
    {
        if (transform.localScale.y < 0 && lightningSpawn.localScale.y > 0)
        {
            Vector3 theScale = lightningSpawn.localScale;
            theScale.y *= -1;
            lightningSpawn.localScale = theScale;
        }
    }

    private void HandleAttack()
    {
        if (rb
            && movement.isPlayerDetected()
            && attackAt < Time.time
            && rb.velocity.x < 1.0f && rb.velocity.x > -1.0f)
        {
            animator.SetTrigger("Attack");
            movement.freeze();
            attackAt = Time.time + coolDown;
        }
    }

    public void shootLightning()
    {
        GameObject temp = Instantiate(lightning, lightningSpawn.position, lightningSpawn.rotation) as GameObject;
        FindObjectOfType<AudioManager>().PlaySound("zLightning");
    }
}
