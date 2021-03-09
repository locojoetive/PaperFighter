using System;
using UnityEngine;

public class FloatingJoystick : MonoBehaviour
{
    private bool useInGameControls;
    private Vector2 vNeutralScreenPosition;
    public bool right = false,
        left = false,
        up = false,
        down = false;

    private int fingerId = -1;
    public float xAxis = 0f;
    public bool fingerDown;
    public float fNeutralFingerPosition = 0f;
    public  float swipeToAxisRatio;
    public  float fHorizontalPosition;
    private Touch[] touches = new Touch[0];

    private void Update()
    {
        vNeutralScreenPosition = .5f * new Vector2(Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight);
        fNeutralFingerPosition = vNeutralScreenPosition.x * 2f / 5f;
        swipeToAxisRatio = fNeutralFingerPosition / 2f;

        useInGameControls = StageManager.onStage && !UIManager.paused;
        if (useInGameControls)
        {
            HandleTouchInputsInGame();
        }
    }

    private void HandleTouchInputsInGame()
    {
        foreach (Touch touch in Input.touches)
        {

            if (fingerId == -1 && touchBegan(touch) && touch.position.x <= vNeutralScreenPosition.x)
            {
                OnPointerDown(touch);
            }
            else if (fingerId == touch.fingerId && touchKept(touch))
            {
                OnDrag(touch);
            }
        }
        foreach (Touch touch in touches)
        {
            if (fingerId == touch.fingerId && touchEnded(touch))
            {
                OnPointerUp(touch);
            }
        }
        touches = Input.touches;
    }


    public virtual void OnPointerDown(Touch touch)
    {
        fingerDown = true;
        fingerId = touch.fingerId;
        OnDrag(touch);
    }
    public void OnDrag(Touch touch)
    {
        fHorizontalPosition = touch.position.x - fNeutralFingerPosition;
        xAxis = Mathf.Sign(fHorizontalPosition) * Mathf.Clamp(
            Mathf.Abs(fHorizontalPosition) / swipeToAxisRatio,
            0f,
            1f
        );

        xAxis = Mathf.Abs(xAxis) == 0f
            ? 0f
            : Mathf.Sign(xAxis) * Mathf.Clamp(Mathf.Abs(xAxis), 0.5f, 1f);
        Moving();
    }

    public virtual void OnPointerUp(Touch touch)
    {
        fHorizontalPosition = touch.position.x - fNeutralFingerPosition;
        fingerDown = false;
        fingerId = -1;
        EndMoving();
    }


    private void Moving()
    {
        right = xAxis > 0F;
        left = xAxis < 0F;
    }

    private void EndMoving()
    {
        xAxis = 0f;
        right = false;
        left = false;
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