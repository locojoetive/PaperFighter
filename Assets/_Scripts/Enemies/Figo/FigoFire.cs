using UnityEngine;

public class FigoFire : MonoBehaviour {

    Collider2D OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponentInParent<PlayerStateAnimationSound>().HandleBurnDamage();
        }
        return other;
    }

}