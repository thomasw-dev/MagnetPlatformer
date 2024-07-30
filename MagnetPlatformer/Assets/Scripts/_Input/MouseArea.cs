using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnLeftButtonDown;
    public event Action OnLeftButtonUp;
    public event Action OnRightButtonDown;
    public event Action OnRightButtonUp;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftButtonDown?.Invoke();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightButtonDown?.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftButtonUp?.Invoke();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightButtonUp?.Invoke();
        }
    }
}
