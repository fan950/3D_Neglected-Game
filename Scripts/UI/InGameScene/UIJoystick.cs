using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Image moveBtnImg;
    private RectTransform rectTransform;
    private Vector2 vecDir = Vector2.zero;
    private float m_fLimit = 130;
    public Vector2 dir
    {
        get { return vecDir.normalized; }
    }
    public void Init()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void Joystick_Move(PointerEventData eventData)
    {
        Vector2 localPosition = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPosition
            );

        moveBtnImg.transform.localPosition = localPosition;

        vecDir = localPosition.normalized;
        if (localPosition.magnitude > m_fLimit)
        {
            moveBtnImg.transform.localPosition = vecDir * m_fLimit;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        Joystick_Move(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Joystick_Move(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        vecDir = Vector2.zero;
        moveBtnImg.rectTransform.anchoredPosition = Vector2.zero;
    }
}
