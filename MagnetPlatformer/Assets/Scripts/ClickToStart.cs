using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickToStart : MonoBehaviour, IPointerClickHandler
{
    public static event Action OnClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }
}