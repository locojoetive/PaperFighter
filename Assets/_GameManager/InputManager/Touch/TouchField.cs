using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class TouchField : MonoBehaviour
{
    private RectTransform rectTransform = null;
    private Canvas canvas;
    private Camera cam;

    private Vector2 vSwipeStart;

    [HideInInspector]
    public Vector2 vSwipe;
    private Vector2 vNeutralScreenPosition;
    private bool useInGame;
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
    private Touch[] touches = new Touch[0];

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
        useInGame = StageManager.onStage && !UIManager.paused;
        // re-check screen size
        vNeutralScreenPosition = .5f * new Vector2(Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight);

        if (useInGame) HandleAction();
        else if (confirm) confirm = false;
    }

    private void HandleAction()
    {
        HandleTouchInputsInGame();


        if (fingerDown)
        {
            // States
            inTime = Time.time < tSwipeCancledAt;
            inRadius = inRadius && vSwipe.magnitude < fSwipeRadius;
        }

        bool jumping = jump || jumpContinuous ||jumpRelease;
        if (actionInProgress && !jumping && inRadius && (fingerDown && !inTime || !fingerDown && inTime))
        {
            jump = true;
            Debug.Log("0_JUMP!");
        }
        else if (jump)
        {
            jump = false;
            jumpContinuous = true;
            Debug.Log("1_JUMP!");
            player.Jump();
        } else if (jumpContinuous)
        {
            jump = false;
            jumpContinuous = false;
            jumpRelease = true;
            Debug.Log("2_JUMP!");
        } else if (jumpRelease)
        {
            jump = false;
            jumpContinuous = false;
            jumpRelease = false;
            actionInProgress = false;
            Debug.Log("3_JUMP!");
        }

        bool swiping = swipe || swipeContinuous;

        if (actionInProgress && fingerDown && !inRadius && !swiping)
        {
            swipe = true;
            Debug.Log("0_SWIPE!");
        }
        else if (swipe)
        {
            swipe = false;
            swipeContinuous = true;
            Debug.Log("1_SWIPE!");
        }
        else if (swipeContinuous)
        {
            swipe = false;
            swipeContinuous = false;
            swipeRelease = true;
            Debug.Log("2_SWIPE!");
        }
        else if (swipeRelease)
        {
            swipe = false;
            swipeContinuous = false;
            swipeRelease = false;
            actionInProgress = false;
            Debug.Log("3_SWIPE!");
        }
    }

    public virtual void OnPointerDown(Touch touch)
    {
        inRadius = true;
        inTime = true;
        fingerDown = true;
        actionInProgress = true;
        fingerId = touch.fingerId;

        tSwipeCancledAt = Time.time + tSwipeCancledAfter;
        vSwipeStart = touch.position;
        vSwipe = Vector2.zero;
                
        OnDrag(touch);
    }
    public void OnDrag(Touch touch)
    {
        
        vSwipe = touch.position - vSwipeStart;
    }

    public virtual void OnPointerUp(Touch touch)
    {
        vSwipe = touch.position - vSwipeStart;

        fingerDown = false;
        fingerId = -1;
    }

    public void OnLevelFinishedLoading()
    {
        useInGame = StageManager.onStage;
        player = FindObjectOfType<PlayerMovement>();
    }

    private void HandleTouchInputsInGame()
    {
        foreach (Touch touch in Input.touches)
        {

            if (touchBegan(touch) && touch.position.x >= vNeutralScreenPosition.x && fingerId == -1)
            {
                OnPointerDown(touch);
            }
            else if (touchKept(touch) && touch.fingerId == fingerId)
            {
                OnDrag(touch);
            }
        }
        foreach (Touch touch in touches) {
            if (touchEnded(touch) && touch.fingerId == fingerId)
                OnPointerUp(touch);
        }
        touches = Input.touches;
    }

    private bool touchBegan(Touch touch)
    {
        foreach (Touch t in touches)
        {
            if (t.fingerId == touch.fingerId)
            {
                return false;
            }
        }
        return true;
    }

    private bool touchKept(Touch touch)
    {
        foreach (Touch t in touches)
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
