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

    private UnityEngine.UI.Image buttonImage;
    private bool isPressed = false;
    private bool hasWaitedOneFrame = false;
    private int currentFingerId = -1;

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
            if (!hasWaitedOneFrame)
            {
                hasWaitedOneFrame = true;
            }
            else
            {
                jump = false;
                jumpContinuous = true;
            }
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
    }

    public void OnPointerDown(BaseEventData eventData)
    {
        if(!jump && !jumpContinuous && !jumpRelease)
        {
            isPressed = true;
            jump = true;
            jumpContinuous = false;
            jumpRelease = false;
            StartCoroutine(JumpPressed());
        }
    }


    System.Collections.IEnumerator JumpPressed()
    {
        isPressed = false;
        float time = 0f;
        while(!isPressed && time < changeColorIn)
        {
            float factor = time / changeColorIn;
            buttonImage.color = Color.Lerp(pressed, neutral, factor);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
