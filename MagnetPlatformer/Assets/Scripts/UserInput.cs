using System;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.EventSystems.PointerEventData;

public class UserInput : MonoBehaviour
{
    [SerializeField] MouseClick mouseClickArea;

    public static event Action MoveLeft;
    public static event Action MoveRight;
    public static event Action Jump;

    public static event Action MagnetSetChargePositive;
    public static event Action MagnetSetChargeNegative;

    // Tunables
    private const InputButton POSITIVE_CHARGE = InputButton.Left;
    private const InputButton NEGATIVE_CHARGE = InputButton.Right;

    void OnEnable()
    {
        Jump += DoJump;
    }

    void OnDisable()
    {
        Jump -= DoJump;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump?.Invoke();
        }
    }

    public void OnMouseClick()
    {
        InputButton click = mouseClickArea.eventData.button;
        if (click == POSITIVE_CHARGE)
        {
            MagnetSetChargePositive?.Invoke();
        }
        if (click == NEGATIVE_CHARGE)
        {
            MagnetSetChargeNegative?.Invoke();
        }
    }

    public static bool Jump_Dev()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public static void DoJump()
    {
        Debug.Log("Player jumped!");
    }

    public static bool IsMovingLeft()
    {
        return Input.GetKey(KeyCode.A);
    }

    public static bool IsMovingRight()
    {
        return Input.GetKey(KeyCode.D);
    }
}