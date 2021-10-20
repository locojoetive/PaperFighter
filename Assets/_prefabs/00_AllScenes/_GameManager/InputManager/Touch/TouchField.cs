using UnityEngine;


public class TouchField : MonoBehaviour
{
    private GameObject joystick;
    private JumpButton jumpButton;
    private Vector2 vSwipeStart;
    
    private Vector2 vNeutralScreenPosition;
    private float tSwipeCancledAt;
    public float tSwipeCancledAfter;
    public float fSwipeRadius;

    internal Vector2 vSwipe;
    
    [SerializeField]
    private bool
        swipe = false,
        swipeContinuous = false,
        swipeRelease = false,
        confirm = false,
        inRadius = false,
        inTime = false,
        fingerDown = false,
        actionInProgress = false;



    private int fingerId = -1;
    private Vector2 fingerPosition;
    private Touch[] oldTouches = new Touch[0];

    public bool Jump { get { return jumpButton.Jump; } }
    public bool JumpContinuous { get { return jumpButton.JumpContinuous; } }
    public bool JumpRelease { get { return jumpButton.JumpRelease; } }
    public bool Swipe { get { return swipe; } }
    public bool SwipeContinuous { get { return swipeContinuous; } }
    public bool SwipeRelease{ get { return swipeRelease; } }
    public bool Confirm { get { return confirm; } }
    public bool FingerDown { get { return fingerDown; } }
    public Vector2 FingerPosition { get { return fingerPosition; } }


    protected virtual void Start()
    {
        jumpButton = GetComponentInChildren<JumpButton>(true);
        joystick = FindObjectOfType<FloatingJoystick>(true).gameObject;
    }

    private void Update()
    {
        vNeutralScreenPosition = .5f * new Vector2(Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight);
        jumpButton.gameObject.SetActive(InputManager.touchActive && InputManager.useInGameControls);
        joystick.SetActive(InputManager.touchActive && InputManager.useInGameControls);
        if (!InputManager.useInGameControls) confirm = ATouchBegan();
        else HandleAction();
        oldTouches = Input.touches;
    }

    private void HandleAction()
    {
        HandleTouchInputsInGame();

        if (fingerDown)
        {
            inTime = Time.time < tSwipeCancledAt;
            inRadius = inRadius && vSwipe.magnitude < fSwipeRadius;
        }
        bool swiping = swipe || swipeContinuous;

        if (actionInProgress && fingerDown && !swiping && !inRadius)
        {
            swipe = true;
        }
        else if (swipe)
        {
            swipe = false;
            swipeContinuous = true;
        }
        else if (swipeContinuous && !fingerDown)
        {
            swipe = false;
            swipeContinuous = false;
            swipeRelease = true;
        }
        else if (swipeRelease)
        {
            swipe = false;
            swipeContinuous = false;
            swipeRelease = false;
            actionInProgress = false;
        }
    }

    public virtual void OnPointerDown(Touch touch)
    {
        inRadius = true;
        inTime = true;
        fingerDown = true;
        actionInProgress = true;
        fingerId = touch.fingerId;
        fingerPosition = touch.position;
        tSwipeCancledAt = Time.time + tSwipeCancledAfter;
        vSwipeStart = touch.position;
        vSwipe = Vector2.zero;
                
        OnDrag(touch);
    }
    public void OnDrag(Touch touch)
    {
        vSwipe = touch.position - vSwipeStart;
        fingerPosition = touch.position;
    }

    public virtual void OnPointerUp(Touch touch)
    {
        vSwipe = touch.position - vSwipeStart;
        fingerDown = false;
        fingerId = -1;
        fingerPosition = touch.position;
    }

    private void HandleTouchInputsInGame()
    {
        foreach (Touch touch in Input.touches)
        {

            if (fingerId == -1 && touchBegan(touch) && touch.position.x >= vNeutralScreenPosition.x)
            {
                OnPointerDown(touch);
            }
            else if (fingerId == touch.fingerId && touchKept(touch))
            {
                OnDrag(touch);
            }
        }
        foreach (Touch touch in oldTouches) {
            if (touch.fingerId == fingerId && touchEnded(touch))
                OnPointerUp(touch);
        }
    }

    private bool ATouchBegan()
    {
        foreach (Touch t in Input.touches)
        {
            if (touchBegan(t))
            {
                return true;
            }
        }
        return false;
    }

    private bool touchBegan(Touch touch)
    {
        foreach (Touch oldTouch in oldTouches)
        {
            if (oldTouch.fingerId == touch.fingerId)
            {
                return false;
            }
        }
        return true;
    }

    private bool touchKept(Touch touch)
    {
        foreach (Touch t in oldTouches)
        {
            if (t.fingerId == touch.fingerId)
            {
                return true;
            }
        }
        return false;
    }

    private bool touchEnded(Touch touch)
    {
        foreach (Touch t in Input.touches)
        {
            if (t.fingerId == touch.fingerId)
            {
                return false;
            }
        }
        return true;
    }
}
