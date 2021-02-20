using UnityEngine;

public class KungFrogMovement : MonoBehaviour {

    private Rigidbody2D rb;
    private KungFrogStatesAnimationSound state;

    public Transform gloveSpawn;
    public GameObject glove;
    private GameObject gloveClone = null;

    public bool jump, facingRight = true;
    public float 
        speed,
        jumpWidth,
        jumpHeigth,
        nextAttackAfter = 2.5f,
        nextAttackAt = 0F;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        state = GetComponent<KungFrogStatesAnimationSound>();
    }

    void Update ()
    {
        if (!state.isDead())
        {
            HandleMovement();
            if (state.isFightStarted()) HandleAttack();
        }
	}

    void HandleMovement()
    {
        
        if (state.isOnWall() || (state.isGrounded() && state.playerDetected() && !state.isPlayerTooClose() && !isFacingPlayer())) Flip();
        if (state.isPlayerTooClose() || !state.playerDetected() && state.isFightStarted()) state.initializeJump();  
    }

    private void Jump()
    {
        rb.velocity = new Vector2(jumpWidth, jumpHeigth);
        FindObjectOfType<AudioManager>().PlaySound("kfJump");
    }

    void Flip()
    {
        facingRight = !facingRight;
        speed = facingRight ? Mathf.Abs(speed) : -Mathf.Abs(speed);
        jumpWidth = facingRight ? Mathf.Abs(jumpWidth) : -Mathf.Abs(jumpWidth);
        rb.velocity = Vector3.zero;
        transform.localScale = new Vector3(
            facingRight ? Mathf.Abs(transform.localScale.x) : - Mathf.Abs(transform.localScale.x),
            transform.localScale.y, 
            transform.localScale.z
        );
    }


    void HandleAttack()
    {
        if (state.isPlayerTooClose())
        {
            state.initializeJump();
        } else if (state.playerDetected() && state.isGrounded() && Time.time > nextAttackAt)
        {
            rb.velocity = new Vector3(0, 0, 0);
            state.initialzeAttack();
            nextAttackAt = Time.time + nextAttackAfter;    
        }
    }

    private bool isFacingPlayer()
    {
        float direction = state.getPlayerPosition().x - transform.position.x;
        return direction > 0 && facingRight || direction < 0 && !facingRight;
    }

    public void SpawnGlove()
    {
        gloveClone = Instantiate(glove, gloveSpawn.transform.position, Quaternion.identity);
        gloveClone.transform.localScale = PointwiseMultiply(gloveClone.transform.localScale, new Vector3(facingRight ? 1F : -1F, 1F, 1F));
    }

    private Vector3 PointwiseMultiply(Vector3 vector, Vector3 multiplyBy)
    {
        return new Vector3(
            vector.x * multiplyBy.x,
            vector.y * multiplyBy.y,
            vector.z * multiplyBy.z
        );
    }
}
