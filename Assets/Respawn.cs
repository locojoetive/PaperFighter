using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Vector3 startPosition;
    internal Quaternion startRotation;
    internal Rigidbody2D rb;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        rb = GetComponent<Rigidbody2D>();
    }

    internal void Reset()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
}
