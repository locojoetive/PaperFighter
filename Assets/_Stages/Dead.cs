using UnityEngine;

public class Dead : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
            other.GetComponent<PlayerStateAnimationSound>().Die();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "groundWall")
        {
            other.GetComponent<Respawn>().Reset();
        }
    }
}
