using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStick : LocalSingleton<JoyStick>, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform joystickBgRect;
    [SerializeField] private Image joystickImg;

    public Vector2 JoystickAnchorPos => joystickImg.rectTransform.anchoredPosition;
    public float JoyStickMinRange { get; } = 7f;    // 조이스틱 인식 최소 범위

    public void OnPointerDown(PointerEventData eventData)
    {
        joystickImg.rectTransform.DOClampJoyStick(joystickBgRect, eventData.position, 0.1f);
    }
    public void OnDrag(PointerEventData eventData)
    {
        joystickImg.rectTransform.ClampJoyStick(joystickBgRect, eventData.position);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        joystickImg.rectTransform.anchoredPosition = Vector2.zero;
    }
}
