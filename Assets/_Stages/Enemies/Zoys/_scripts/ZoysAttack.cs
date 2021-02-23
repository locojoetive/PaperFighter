using UnityEngine;

public class ZoysAttack : MonoBehaviour {
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
        if (movement.isPlayerDetected() && attackAt < Time.time)
        {
            animator.SetTrigger("Attack");
            movement.freeze();
            attackAt = Time.time + coolDown;
        }
    }

    public void shootLightning()
    {
        Instantiate(lightning, lightningSpawn.position, lightningSpawn.rotation);
        FindObjectOfType<AudioManager>().PlaySound("zLightning");
    }
}
