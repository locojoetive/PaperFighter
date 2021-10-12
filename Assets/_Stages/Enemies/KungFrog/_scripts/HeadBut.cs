using UnityEngine;

public class HeadBut : MonoBehaviour
{
    KungFrogStatesAnimationSound body;

    private void Start()
    {
        body = FindObjectOfType<KungFrogStatesAnimationSound>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && body.life > 0)
        {
            other.gameObject.GetComponent<PlayerStateAnimationSound>().HandleNormalDamage();
        }
    }
}
