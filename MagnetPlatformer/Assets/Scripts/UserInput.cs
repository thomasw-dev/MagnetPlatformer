using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.PointerEventData;

public class UserInput : MonoBehaviour
{
    private const InputButton POSITIVE_CHARGE = InputButton.Left;
    private const InputButton NEGATIVE_CHARGE = InputButton.Right;

    public static event Action OnMoveLeftInput;
    public static event Action OnMoveLeftInputStop;

    public static event Action OnMoveRightInput;
    public static event Action OnMoveRightInputStop;
    public static event Action OnJumpInput;

    public static event Action<Magnet.Charge> OnMagnetSetCharge;

    void OnEnable()
    {
        MagnetMouseControl.OnLeftButtonDown += MagnetSetCharge_Positive;
        MagnetMouseControl.OnLeftButtonUp += MagnetSetCharge_Neutral;
        MagnetMouseControl.OnRightButtonDown += MagnetSetCharge_Negative;
        MagnetMouseControl.OnRightButtonUp += MagnetSetCharge_Neutral;
    }

    void OnDisable()
    {
        MagnetMouseControl.OnLeftButtonDown -= MagnetSetCharge_Positive;
        MagnetMouseControl.OnLeftButtonUp -= MagnetSetCharge_Neutral;
        MagnetMouseControl.OnRightButtonDown -= MagnetSetCharge_Negative;
        MagnetMouseControl.OnRightButtonUp -= MagnetSetCharge_Neutral;
    }

    void Update()
    {
        // Move Left
        if (Input.GetKey(KeyCode.A))
        {
            OnMoveLeftInput?.Invoke();
        }
        else OnMoveLeftInputStop?.Invoke();

        // Move Right
        if (Input.GetKey(KeyCode.D))
        {
            OnMoveRightInput?.Invoke();
        }
        else OnMoveRightInputStop?.Invoke();

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpInput?.Invoke();
        }
    }

    void MagnetSetCharge_Neutral() => OnMagnetSetCharge?.Invoke(Magnet.Charge.Neutral);
    void MagnetSetCharge_Positive() => OnMagnetSetCharge?.Invoke(Magnet.Charge.Positive);
    void MagnetSetCharge_Negative() => OnMagnetSetCharge?.Invoke(Magnet.Charge.Negative);
}