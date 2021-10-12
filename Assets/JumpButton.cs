using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour
{
    [SerializeField]
    private Color neutral;
    [SerializeField]
    private Color pressed;

    [SerializeField]
    private bool jump;
    [SerializeField]
    private bool jumpContinuous;
    [SerializeField]
    private bool jumpRelease;
    [SerializeField]
    private float changeColorIn = 1f;
    private int currentFingerId = int.MaxValue;

    private UnityEngine.UI.Image buttonImage;
    private bool isPressed = false;
    private float changeColorTime;

    public bool Jump { get { return jump; } }
    public bool JumpContinuous { get { return jumpContinuous; } }
    public bool JumpRelease { get { return jumpRelease; } }

    void Start()
    {
        buttonImage = GetComponent<UnityEngine.UI.Image>();
    }

    private void Update()
    {
        if (jump)
        {
            jump = false;
            jumpContinuous = true;
        }
        else if (jumpContinuous)
        {
            jumpContinuous = false;
            jumpRelease = true;
        }
        else if (jumpRelease)
        {
            jumpRelease = false;
        }
        bool fingerStillPresent = false;
        foreach (Touch touch in Input.touches)
        {
            if (touch.fingerId == currentFingerId)
            {
                fingerStillPresent = true;
                break;
            }
        }
        if (!fingerStillPresent)
        {
            OnPointerUp();
        }
    }

    public void OnPointerDown(BaseEventData eventData)
    {
        if (currentFingerId == int.MaxValue)
        {
            currentFingerId = ((PointerEventData)eventData).pointerId;
            Debug.Log("Finger " + currentFingerId + " entered!");
            if (!isPressed)
            {
                StartCoroutine(JumpPressed());
            } else
            {
                changeColorTime = 0f;
            }
            Debug.Log("JUMP!");
            jump = true;
            jumpContinuous = false;
            jumpRelease = false;
        }
    }

    public void OnPointerUp()
    {
        if (currentFingerId != int.MaxValue)
        {
            Debug.Log("Finger " + currentFingerId + " released!");
            currentFingerId = int.MaxValue;
        }
    }


    System.Collections.IEnumerator JumpPressed()
    {
        isPressed = true;
        buttonImage.color = pressed;
        changeColorTime = 0f;
        while (changeColorTime < changeColorIn)
        {
            float factor = changeColorTime / changeColorIn;
            buttonImage.color = Color.Lerp(pressed, neutral, factor);
            changeColorTime += Time.deltaTime;
            yield return null;
        }
        isPressed = false;
    }
}
