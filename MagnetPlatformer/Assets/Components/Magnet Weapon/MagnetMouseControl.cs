using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MagnetMouseControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static event Action OnLeftButtonDown;
    public static event Action OnLeftButtonUp;
    public static event Action OnRightButtonDown;
    public static event Action OnRightButtonUp;

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