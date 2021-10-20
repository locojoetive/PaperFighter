using UnityEngine;

public class GravityManipulator : MonoBehaviour {

	Collider2D OnTriggerEnter2D(Collider2D other)
    {
        GravityReverse script = other.GetComponent<GravityReverse>();
        if(script) script.ReverseGravity();
        return other;
    }

    Collider2D OnTriggerExit2D(Collider2D other)
    {
        GravityReverse script = other.GetComponent<GravityReverse>();
        if (script) script.CorrectGravity();
        return other;
    }
}
