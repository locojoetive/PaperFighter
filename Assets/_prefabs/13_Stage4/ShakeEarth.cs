using System.Collections.Generic;
using UnityEngine;

public class ShakeEarth : MonoBehaviour
{
    List<GameObject> alreadyHit = new List<GameObject>();

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "groundWall" && !alreadyHit.Contains(other.gameObject))
        {
            alreadyHit.Add(other.gameObject);
            StartCoroutine(FindObjectOfType<CameraShake>().Shake(0.2f, 0.1f));
        }
    }
}
