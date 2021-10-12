using UnityEngine;

public class FigoFire : MonoBehaviour {

    Collider2D OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerStateAnimationSound player = other.GetComponentInParent<PlayerStateAnimationSound>();
            player.HandleBurnDamage();
        }
        return other;
    }

}