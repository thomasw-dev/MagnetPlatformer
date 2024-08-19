using System;
using UnityEngine.EventSystems;

public class MagnetGunMouseArea : MouseArea
{
    public event Action<Magnet.Charge> OnMagnetSetCharge;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnMagnetSetCharge?.Invoke(Magnet.Charge.Positive);
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnMagnetSetCharge?.Invoke(Magnet.Charge.Negative);
        }
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
    }
}
