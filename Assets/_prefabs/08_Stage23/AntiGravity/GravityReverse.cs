using UnityEngine;

public class GravityReverse : MonoBehaviour {
    float regularGravity = 2;
    float reverseGravity = -2;

    private Rigidbody2D rb;
    private bool upsidedown = false;
    private bool isObjectAFly = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!isObjectAFly)
            rb.gravityScale = regularGravity;
    }

    public void ReverseGravity()
    {
        upsidedown = true;
        Vector3 theScale = transform.localScale;
        theScale.y = -Mathf.Abs(theScale.y);
        transform.localScale = theScale;
        if (!isObjectAFly)
        {
            if (gameObject.tag == "Player") GetComponent<PlayerStateAnimationSound>().setUpsideDown(true);
            if (rb != null) rb.gravityScale = reverseGravity;
        }
    }

    public void CorrectGravity()
    {
        upsidedown = false;
        Vector3 theScale = transform.localScale;
        theScale.y = Mathf.Abs(theScale.y);
        transform.localScale = theScale;

        if (!isObjectAFly)
        {
            if (gameObject.tag == "Player") GetComponent<PlayerStateAnimationSound>().setUpsideDown(false);
            if (rb != null) rb.gravityScale = regularGravity;
        }
    }

    public void ObjectIsAFly()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        isObjectAFly = true;
    }

    public bool isUpsidedown()
    {
        return upsidedown;
    }

    public float getGravity()
    {
        return rb.gravityScale;
    }
}
