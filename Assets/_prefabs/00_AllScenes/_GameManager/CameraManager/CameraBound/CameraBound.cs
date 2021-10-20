using UnityEngine;

public class CameraBound : MonoBehaviour
{
    public System.Collections.Generic.List<Bounds> bounds;
    void Start()
    {
        bounds = new System.Collections.Generic.List<Bounds>();
        foreach (BoxCollider2D collider in GetComponents<BoxCollider2D>())
        {
            bounds.Add(collider.bounds);
        }
    }
}
