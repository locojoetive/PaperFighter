using UnityEngine;
using UnityEngine.UI;

public class ArrowColors : MonoBehaviour
{
    [SerializeField] private Image left;
    [SerializeField] private Image right;
    private FloatingJoystick joystick;
    public Color full;
    public Color empty;

    void Start()
    {
        joystick = GetComponent<FloatingJoystick>();
        left.color = empty;
        right.color = empty;
    }

    private void Update()
    {
        float xAxis = joystick.xAxis;
        left.color = Color.Lerp(empty, full, -xAxis);
        right.color = Color.Lerp(empty, full, xAxis);
    }
}
