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

    public static event Action<Magnet.Charge> OnMagnetSetCharge;

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
        if (GameManager.GameState == GameManager.State.Initialization)
        {
            if (CheckAnyKeyInput())
            {
                OnAnyKeyInput?.Invoke();
            }
        }

        if (GameManager.GameState == GameManager.State.Playing)
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
        if (GameManager.GameState != GameManager.State.Playing) { return; }
        OnMagnetSetCharge?.Invoke(Magnet.Charge.Neutral);
    }
    void MagnetSetCharge_Positive()
    {
        if (GameManager.GameState != GameManager.State.Playing) { return; }
        OnMagnetSetCharge?.Invoke(Magnet.Charge.Positive);
    }
    void MagnetSetCharge_Negative()
    {
        if (GameManager.GameState != GameManager.State.Playing) { return; }
        OnMagnetSetCharge?.Invoke(Magnet.Charge.Negative);
    }
}