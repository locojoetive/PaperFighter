using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    private int avgFrameRate;
    private Text display_Text;
    
    void Start()
    {
        display_Text = GetComponent<Text>();
    }

    public void Update()
    {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;
        display_Text.text = avgFrameRate.ToString() + " FPS";
    }
}
