using UnityEngine;

public class ShurikenScript : MonoBehaviour {

    public int damage;
    public bool damaged = false;

    public bool isEnemyShot = false;
    private static bool facingRight = true;
    public float speed;
    private float actualRotation;
    public float baseRotation;
    private Animator animator;
    public Vector3 velocity;

    void Start() {
        animator = GetComponent<Animator>();
        Destroy(gameObject, 10F);
    }

    void Update () {
        if (facingRight)
            transform.Rotate(0.0f, 0.0f, actualRotation, Space.Self);
        else
            transform.Rotate(0.0f, 0.0f, -actualRotation, Space.Self);
    }

    public void setDirection(bool right){
        facingRight = right;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "groundWall" || other.gameObject.tag == "Player")
        {
            animator.SetTrigger("Damaged");
            damaged = true;
            Destroy(GetComponent<GravityReverse>());
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(gameObject, 0.5f);
            GetComponent<CircleCollider2D>().isTrigger = true;
        }
        else if ((other.gameObject.tag == "projectileA" || other.gameObject.tag == "hittable") && !damaged)
        {
            StartCoroutine(FindObjectOfType<CameraShake>().Shake(.1f, .1f));
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Destroy(gameObject);
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        actualRotation = baseRotation * velocity.magnitude / 4f;
        this.velocity = velocity;
        if (velocity.x == 0 && velocity.y == 0 && velocity.z == 0)
        {
            if (facingRight)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector3(0.5f * speed, 0.1f * speed, 0.0f);
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector3(0.5f * -speed, 0.1f * speed, 0.0f);
            }
        }
        else
            GetComponent<Rigidbody2D>().velocity = velocity * speed;
    }

}
