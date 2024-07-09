using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static event Action OnAnyKeyInput;

    public static event Action OnMoveLeftInput;
    public static event Action OnMoveLeftInputStop;

    public static event Action OnMoveRightInput;
    public static event Action OnMoveRightInputStop;
    public static event Action OnJumpInput;

    public static event Action<Magnet.Charge> OnMagnetWeaponSetCharge;

    const KeyCode MOVE_LEFT_KEY = KeyCode.A;
    const KeyCode MOVE_RIGHT_KEY = KeyCode.D;
    const KeyCode JUMP_KEY = KeyCode.Space;


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
        if (GameState.CurrentState == GameState.Initialize)
        {
            if (CheckAnyKeyInput())
            {
                OnAnyKeyInput?.Invoke();
            }
        }

        if (GameState.CurrentState == GameState.Play)
        {
            MovementInput();
        }
    }

    bool CheckAnyKeyInput()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) return false;
            return true;
        }
        else return false;
    }

    void MovementInput()
    {
        // Move Left
        if (Input.GetKey(MOVE_LEFT_KEY))
        {
            OnMoveLeftInput?.Invoke();
        }
        else OnMoveLeftInputStop?.Invoke();

        // Move Right
        if (Input.GetKey(MOVE_RIGHT_KEY))
        {
            OnMoveRightInput?.Invoke();
        }
        else OnMoveRightInputStop?.Invoke();

        // Jump
        if (Input.GetKeyDown(JUMP_KEY))
        {
            OnJumpInput?.Invoke();
        }
    }

    void MagnetSetCharge_Neutral()
    {
        if (GameState.CurrentState != GameState.Play) { return; }
        OnMagnetWeaponSetCharge?.Invoke(Magnet.Charge.Neutral);
    }
    void MagnetSetCharge_Positive()
    {
        if (GameState.CurrentState != GameState.Play) { return; }
        OnMagnetWeaponSetCharge?.Invoke(Magnet.Charge.Positive);
    }
    void MagnetSetCharge_Negative()
    {
        if (GameState.CurrentState != GameState.Play) { return; }
        OnMagnetWeaponSetCharge?.Invoke(Magnet.Charge.Negative);
    }
}