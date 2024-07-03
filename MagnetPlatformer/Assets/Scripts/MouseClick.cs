using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseClick : MonoBehaviour, IPointerClickHandler
{
    public static event Action OnLeftClick;
    public static event Action OnRightClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick?.Invoke();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick?.Invoke();
        }
    }
}