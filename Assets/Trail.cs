using UnityEngine;

public class Trail : MonoBehaviour
{
    private TouchField touchField;
    private TrailRenderer trail;
    private bool clear = false;
    void Start()
    {
        touchField = FindObjectOfType<TouchField>();
        trail = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (touchField.fingerDown)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(touchField.fingerPosition);
            transform.position = pos;
            if (clear)
            {
                trail.Clear();
                clear = false;
            }
        }
        else
        {
            clear = true;
        }
    }
}
