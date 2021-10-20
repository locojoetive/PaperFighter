using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float startPosition;
    public float endPosition;
    public float speed;
    public bool goUp = false;
    public float continueMovingAfter = 1f;
    public float continueMovingAt = 0f;

    void Update()
    {
        if (continueMovingAt < Time.time)
        {
            if (goUp && transform.localPosition.y < endPosition)
            {
                transform.Translate(Vector2.up * speed);
            }
            else if (!goUp && transform.localPosition.y > startPosition)
            {
                transform.Translate(-Vector2.up * speed);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            goUp = false;
            collision.collider.transform.parent = null;
            continueMovingAt = Time.time + continueMovingAfter;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            bool isGroundedOnPlatform = GetComponent<Collider2D>().Equals(collision.collider.GetComponent<PlayerStateAnimationSound>().groundedOn);
            if (isGroundedOnPlatform)
            {
                //if (!goUp)
                  //  continueMovingAt = Time.time + continueMovingAfter;


                goUp = true;
                collision.collider.transform.parent = transform;
            } else
            {
                goUp = false;
                collision.collider.transform.parent = null;
                continueMovingAt = Time.time + continueMovingAfter;
            }
        }
    }
}
