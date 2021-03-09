using UnityEngine;


public class TouchField : MonoBehaviour
{
    private RectTransform rectTransform = null;
    private Canvas canvas;
    private Camera cam;

    private Vector2 vSwipeStart;

    [HideInInspector]
    public Vector2 vSwipe;
    private Vector2 vNeutralScreenPosition;
    private bool useInGameControls;
    private float tSwipeCancledAt;
    public float tSwipeCancledAfter;
    public float fSwipeRadius;

    PlayerMovement player;

    public bool
        swipe = false,
        swipeContinuous = false,
        swipeRelease = false,
        jump = false,
        jumpContinuous = false,
        jumpRelease = false,
        confirm = false,
        inRadius = false,
        inTime = false,
        fingerDown = false,
        actionInProgress = false;

    private int fingerId = -1;
    public Vector2 fingerPosition;
    private Touch[] oldTouches = new Touch[0];

    protected virtual void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        cam = null;
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            cam = canvas.worldCamera;

        if (canvas == null)
            Debug.LogError("The Joystick is not placed inside a canvas");
        
        OnLevelFinishedLoading();
    }

    private void Update()
    {
        useInGameControls = StageManager.onStage && !UIManager.paused;
        vNeutralScreenPosition = .5f * new Vector2(Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight);
        
        if (!useInGameControls) confirm = ATouchBegan();
        else HandleAction();
        oldTouches = Input.touches;
    }

    private void HandleAction()
    {
        // DebugTouches();
        HandleTouchInputsInGame();

        if (fingerDown)
        {
            // States
            inTime = Time.time < tSwipeCancledAt;
            inRadius = inRadius && vSwipe.magnitude < fSwipeRadius;
        }

        bool jumping = jump || jumpContinuous ||jumpRelease;
        if (actionInProgress && !fingerDown && !jumping && inRadius)
        {
            jump = true;
        }
        else if (jump)
        {
            jump = false;
            jumpContinuous = true;
            // player.Jump();
        } else if (jumpContinuous)
        {
            jump = false;
            jumpContinuous = false;
            jumpRelease = true;
        }
        else if (jumpRelease)
        {
            jump = false;
            jumpContinuous = false;
            jumpRelease = false;
            actionInProgress = false;
        }

        bool swiping = swipe || swipeContinuous;

        if (actionInProgress && fingerDown && !swiping && !jumping && !inRadius)
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

    public void OnLevelFinishedLoading()
    {
        useInGameControls = StageManager.onStage;
        player = FindObjectOfType<PlayerMovement>();
    }
}
