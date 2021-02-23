using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchBehaviour : MonoBehaviour
{
    public static Touch[] touches;
    public Joystick joystick;
    public static Vector2 swipeVector;

    public float tSwipeIn = 0.05f;
    public int fSwipeRadius = 25;
    public Transform fxTransform;

    public static float xAxis,
        yAxis;
    public static bool active,
        action,
        actionContinuous,
        actionRelease,
        jump,
        jumpContinuous,
        jumpRelease,
        up,
        right,
        left,
        down,
        escape,
        confirm,
        reset,
        swipe,
        inGame;

    private float tSwipeCanceledAt = 0F;

    public static int iMovingFinger = -1,
        iJumpingFinger = -1,
        iSwipingFinger = -1,
        iActionOrJumpFinger = -1;

    public static Vector2 neutralScreenPosition;
    private bool useInGameControls;

    private void Start()
    {
        touches = new Touch[0];
        Input.multiTouchEnabled = true;
        neutralScreenPosition = .5f * new Vector2(Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight);
    }

    internal void Update()
    {
        active = InputManager.touchActive && InputManager.active;
        useInGameControls = StageManager.onStage && !UIManager.paused;
        joystick.gameObject.SetActive(useInGameControls);

        if (active)
        {
            neutralScreenPosition = .5f * new Vector2(Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight);
            ResetJumpAndAction();

            if (useInGameControls) 
            { 
                HandleTouchInputsInGame();
            }
            else
            {
                HandleTouchInputsInMenu();
            }
        }
    }

    private void ResetButtons()
    {
        action = false;
        actionContinuous = false;
        actionRelease = false;
        jump = false;
        jumpContinuous = false;
        jumpRelease = false;
        up = false;
        down = false;
        left = false;
        right = false;
        swipe = false;
        confirm = false;
        
        iJumpingFinger = -1;
        iSwipingFinger = -1;
        iActionOrJumpFinger = -1;

        tSwipeCanceledAt = 0F;
        xAxis = 0F;
        yAxis = 0F;
    }

    private void ResetJumpAndAction()
    {
        if (iActionOrJumpFinger == -1)
        {
            if (iJumpingFinger == -1)
            {
                if (jumpRelease)
                {
                    jumpRelease = false;
                }
                else if (jump)
                {
                    jump = false;
                    jumpRelease = true;
                }
            }
            if (iSwipingFinger == -1)
            {
                if (actionRelease)
                {
                    actionRelease = false;
                }
                else if (action)
                {
                    action = false;
                    actionRelease = true;
                }
            }
        }
    }

    private void HandleTouchInputsInGame()
    {
        foreach (Touch touch in Input.touches)
        {
            
            if (!invokeUIButtonByTouch(touch))
            {

                if(touchBegan(touch))
                {
                    if (iMovingFinger != touch.fingerId && iActionOrJumpFinger == -1 && touch.position.x >= neutralScreenPosition.x)
                    {
                        iActionOrJumpFinger = touch.fingerId;
                        tSwipeCanceledAt = Time.time + tSwipeIn;
                        swipeVector = Vector2.zero;
                    }
                }
                else if(touchKept(touch))
                {
                    if (iMovingFinger == touch.fingerId)
                    {
                        Moving();
                    }
                    else if (iJumpingFinger == touch.fingerId)
                    {
                        Jumping();
                    }
                    else if (iSwipingFinger == touch.fingerId)
                    {
                        Swiping(touch, out action, out actionContinuous);
                    }
                    else if (iActionOrJumpFinger == touch.fingerId)
                    {
                        swipeVector += touch.deltaPosition;
                        if (tSwipeCanceledAt < Time.time)
                        {
                            StarJumping(touch);
                            iActionOrJumpFinger = -1;
                        }
                        else if (tSwipeCanceledAt > Time.time && swipeVector.magnitude > fSwipeRadius)
                        {
                            StartSwiping(touch, out action);
                            iActionOrJumpFinger = -1;
                        }
                    }
                }
            }

        }
        foreach(Touch touch in touches)
            if (touchEnded(touch))
            {
                if (iMovingFinger == touch.fingerId)
                {
                    EndMoving();
                }
                else if (iJumpingFinger == touch.fingerId)
                {
                    EndJumping();
                }
                else if (iSwipingFinger == touch.fingerId)
                {
                    EndSwiping();
                }
                else if (iActionOrJumpFinger == touch.fingerId)
                {
                    if (iJumpingFinger == -1)
                    {
                        jump = true;
                    }
                    iActionOrJumpFinger = -1;
                    swipeVector = Vector2.zero;
                }
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

    private void HandleTouchInputsInMenu()
    {
        if (confirm) confirm = false;
        foreach (Touch touch in Input.touches)
        {
            if (!invokeUIButtonByTouch(touch))
                if (TouchPhase.Began == touch.phase)
                {
                    Confirm();
                }
             
        }
    }

    private void Confirm()
    {
        confirm = true;
    }

    private void StarJumping(Touch touch)
    {
        iJumpingFinger = touch.fingerId;
        jump = true;
    }

    private void StartSwiping(Touch touch, out bool affectedField)
    {
        fxTransform.position = ScreenToWorldPoint(touch.position);
        fxTransform.GetComponent<TrailRenderer>().Clear();
        iSwipingFinger = touch.fingerId;
        affectedField = true;
    }
    private void Moving()
    {
        xAxis = joystick.Horizontal;
        yAxis = joystick.Vertical;

        right = xAxis > 0F;
        left = xAxis < 0F;

        up = yAxis > 0F;
        down = yAxis < 0F;
    }

    private void Jumping()
    {
        jump = false;
        jumpContinuous = true;
    }


    private void Swiping(Touch touch, out bool affectedField1, out bool affectedField2)
    {
        affectedField1 = false;
        affectedField2 = true;
        swipeVector += touch.deltaPosition;
        fxTransform.position = ScreenToWorldPoint(touch.position);
    }
    private void EndMoving()
    {
        iMovingFinger = -1;
        right = false;
        left = false;
        up = false;
        down = false;
        xAxis = 0F;
        yAxis = 0F;
    }

    private void EndJumping()
    {
        iJumpingFinger = -1;
        jumpContinuous = false;
        jumpRelease = true;
    }

    private void EndSwiping()
    {
        iSwipingFinger = -1;
        actionContinuous = false;
        actionRelease = true;
    }

    private Vector3 ScreenToWorldPoint(Vector2 position)
    {
        Vector3 fxPosition = Camera.main.ScreenToWorldPoint(position);
        fxPosition.z = 0F;
        return fxPosition;
    }

    private bool invokeUIButtonByTouch(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = touch.position;
            System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            int buttonCount = 0;
            results.ForEach(delegate (RaycastResult result) {
                DebugTouches debugTouches = FindObjectOfType<DebugTouches>();
                if (debugTouches != null)
                    debugTouches.lastTouched = result.gameObject;
                Button button = result.gameObject.GetComponent<Button>();
                if (button != null)
                {
                    Debug.Log(button.name);
                    button.onClick.Invoke();
                    buttonCount++;
                }
            });
            return buttonCount > 0;
        }
        return false;
    }
}


