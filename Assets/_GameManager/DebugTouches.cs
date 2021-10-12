using UnityEngine;
using UnityEngine.UI;

public class DebugTouches : MonoBehaviour
{
    Text text;
    public GameObject lastTouched;
    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        Touch[] touches = Input.touches;
        string debugText = touches.Length + " Touches!\n";
        if (touches != null && touches.Length > 0)
        {
            foreach(Touch touch in touches)
            {
                debugText += "Touch " + touch.ToString() + ": ";
                debugText += touch.position;
                debugText += "\n";
            }
        }
        Vector2 swipeVector = FindObjectOfType<TouchField>().vSwipe;
        if (swipeVector != null)
        {
            swipeVector.x = swipeVector.x / Camera.main.scaledPixelWidth;
            swipeVector.y = swipeVector.y / Camera.main.scaledPixelWidth;
            debugText += swipeVector;
        }
        
        text.text = debugText;
    }
}
