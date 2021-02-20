using UnityEngine;

public class HeadBut : MonoBehaviour
{
    KungFrogStatesAnimationSound body;

    private void Start()
    {
        body = FindObjectOfType<KungFrogStatesAnimationSound>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerStateAnimationSound>().HandleNormalDamage();
        }
    }
}
