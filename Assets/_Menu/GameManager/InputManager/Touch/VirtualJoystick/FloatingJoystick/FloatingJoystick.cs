using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (TouchBehaviour.active
            && TouchBehaviour.iMovingFinger == -1
            && StageManager.onStage
            && !StageManager.paused
            && eventData.position.x < TouchBehaviour.neutralScreenPosition.x 
        ) {
            TouchBehaviour.iMovingFinger = eventData.pointerId;
            Debug.Log("EVENT BEGAN: " + TouchBehaviour.iMovingFinger);
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
            base.OnPointerDown(eventData);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId == TouchBehaviour.iMovingFinger)
        {
            background.gameObject.SetActive(false);
            Debug.Log("EVENT ENDED: " + TouchBehaviour.iMovingFinger);
            base.OnPointerUp(eventData);
        }
    }
}