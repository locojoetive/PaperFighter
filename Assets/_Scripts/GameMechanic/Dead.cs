using UnityEngine;

public class Dead : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
            other.GetComponent<PlayerStateAnimationSound>().Die();
    }
}
