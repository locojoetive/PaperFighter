using UnityEngine;
using UnityEngine.UI;

public class ArrowColors : MonoBehaviour
{
    [SerializeField] private Image left;
    [SerializeField] private Image right;
    private FloatingJoystick joystick;
    private Color full;
    private Color empty;

    void Start()
    {
        joystick = GetComponent<FloatingJoystick>();
        full = left.color;
        empty = new Color(full.r, full.g, full.b, .5f * full.a);
    }

    private void Update()
    {

        left.gameObject.SetActive(InputManager.touchActive);
        right.gameObject.SetActive(InputManager.touchActive);
        if (InputManager.touchActive)
        {
            float xAxis = joystick.xAxis;

            left.color = Color.Lerp(empty, full, -xAxis);
            right.color = Color.Lerp(empty, full, xAxis);
        }
    }
}
