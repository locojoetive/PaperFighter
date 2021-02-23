using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    private bool useInGameControls;
    private Vector2 vNeutralScreenPosition;
    public bool right = false,
        left = false,
        up = false,
        down = false;

    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    private void Update()
    {
        vNeutralScreenPosition = .5f * new Vector2(Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight);
        useInGameControls = StageManager.onStage && !UIManager.paused;

        if (useInGameControls)
        {
            Moving();
        }
    }

    private void Moving()
    {
        right = Horizontal > 0F;
        left = Horizontal < 0F;

        up = Vertical > 0F;
        down = Vertical < 0F;
    }

    private void EndMoving()
    {
        right = false;
        left = false;
        up = false;
        down = false;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (useInGameControls
            && eventData.position.x < vNeutralScreenPosition.x 
        ) {
            Debug.Log("MOVE EVENT BEGAN: " + TouchBehaviour.iMovingFinger);
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
            base.OnPointerDown(eventData);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        EndMoving();
        Debug.Log("MOVE ENDED: " + TouchBehaviour.iMovingFinger);
        base.OnPointerUp(eventData);
    }
}