using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.PointerEventData;

public class UserInput : MonoBehaviour
{
    [SerializeField] MouseClick mouseClickArea;

    private const InputButton POSITIVE_CHARGE = InputButton.Left;
    private const InputButton NEGATIVE_CHARGE = InputButton.Right;

    public static event Action OnMoveLeftInput;
    public static event Action OnMoveRightInput;
    public static event Action OnJumpInput;

    public static event Action MagnetSetChargePositive;
    public static event Action MagnetSetChargeNegative;

    void OnEnable()
    {
        MouseClick.OnLeftClick += Invoke_MagnetSetChargePositive;
        MouseClick.OnRightClick += Invoke_MagnetSetChargeNegative;
    }

    void OnDisable()
    {
        MouseClick.OnLeftClick -= Invoke_MagnetSetChargePositive;
        MouseClick.OnRightClick -= Invoke_MagnetSetChargeNegative;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            OnMoveLeftInput?.Invoke();
        }
        if (Input.GetKey(KeyCode.D))
        {
            OnMoveRightInput?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpInput?.Invoke();
        }
        if (Input.GetMouseButtonDown(0))
        {
            MagnetSetChargePositive?.Invoke();
        }
        if (Input.GetMouseButtonDown(1))
        {
            MagnetSetChargeNegative?.Invoke();
        }
    }

    /*public void OnPointerClick(PointerEventData eventData)
    {
        //InputButton click = mouseClickArea.eventData.button;
        //Debug.Log(click);
        if (click == POSITIVE_CHARGE)
        {
            MagnetSetChargePositive?.Invoke();
        }
        if (click == NEGATIVE_CHARGE)
        {
            MagnetSetChargeNegative?.Invoke();
        }
    }*/

    void Invoke_MagnetSetChargePositive() => MagnetSetChargePositive?.Invoke();

    void Invoke_MagnetSetChargeNegative() => MagnetSetChargeNegative?.Invoke();
}