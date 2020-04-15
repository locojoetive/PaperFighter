using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBound : MonoBehaviour
{
    public List<Bounds> bounds;
    void Start()
    {
        bounds = new List<Bounds>();
        foreach (BoxCollider2D collider in GetComponents<BoxCollider2D>())
        {
            bounds.Add(collider.bounds);
        }
    }
}
