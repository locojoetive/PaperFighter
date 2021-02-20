using UnityEngine;

public class GravityReverse : MonoBehaviour {
    private float 
        gravity = 9.81f,
        currentGravity = -9.81f;
    private Rigidbody2D rb;
    private bool upsidedown = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + currentGravity * Time.deltaTime);
    }
    public void ReverseGravity()
    {
        currentGravity = 3F * gravity;
        upsidedown = true;
        if (gameObject.tag == "Player") GetComponent<PlayerStateAnimationSound>().setUpsideDown(true);

        Vector3 theScale = transform.localScale;
        theScale.y = -Mathf.Abs(theScale.y);
        transform.localScale = theScale;
    }

    public void CorrectGravity()
    {
        currentGravity = -gravity;
        upsidedown = false;
        if (gameObject.tag == "Player") GetComponent<PlayerStateAnimationSound>().setUpsideDown(false);

        Vector3 theScale = transform.localScale;
        theScale.y = Mathf.Abs(theScale.y);
        transform.localScale = theScale;
    }

    public void ObjectIsAFly()
    {
        currentGravity = 0;
    }

    public bool isUpsidedown()
    {
        return upsidedown;
    }

    public float getGravity()
    {
        return currentGravity;
    }
}
