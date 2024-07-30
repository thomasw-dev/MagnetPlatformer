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

    public static event Action<Magnet.Charge> OnMagnetGunSetCharge;
    public static event Action OnToggleChargeInput;

    const KeyCode MOVE_LEFT_KEY = KeyCode.A;
    const KeyCode MOVE_RIGHT_KEY = KeyCode.D;
    const KeyCode JUMP_KEY = KeyCode.Space;
    const KeyCode JUMP_KEY_ALT = KeyCode.W;
    const KeyCode TOGGLE_CHARGE_KEY = KeyCode.E;

    Magnet.Charge _toggleCharge = Magnet.Charge.Positive;

    void OnEnable()
    {
        GameState.Play.OnEnter += EnterPlay;
    }

    void OnDisable()
    {
        GameState.Play.OnEnter -= EnterPlay;
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
            GameplayInputs();
        }
    }

    void EnterPlay()
    {
        // Initial charge
        OnMagnetGunSetCharge?.Invoke(_toggleCharge);
    }

    bool CheckAnyKeyInput()
    {
        if (Input.anyKeyDown)
        {
            // Exclude Esc from any keys
            if (Input.GetKeyDown(KeyCode.Escape)) return false;
            return true;
        }
        else return false;
    }

    void GameplayInputs()
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
        if (Input.GetKeyDown(JUMP_KEY) || Input.GetKeyDown(JUMP_KEY_ALT))
        {
            OnJumpInput?.Invoke();
        }

        // Toggle Charge (Positive / Negative)
        if (Input.GetKeyDown(TOGGLE_CHARGE_KEY))
        {
            // Flip it between Positive and Negative 
            _toggleCharge = _toggleCharge == Magnet.Charge.Positive ? Magnet.Charge.Negative : Magnet.Charge.Positive;

            OnMagnetGunSetCharge?.Invoke(_toggleCharge);

            OnToggleChargeInput?.Invoke();
        }
    }
}