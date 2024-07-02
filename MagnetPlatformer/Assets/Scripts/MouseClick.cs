using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseClick : MonoBehaviour, IPointerClickHandler
{
    public PointerEventData eventData;

    public void OnPointerClick(PointerEventData eventData)
    {
        this.eventData = eventData;
    }
}