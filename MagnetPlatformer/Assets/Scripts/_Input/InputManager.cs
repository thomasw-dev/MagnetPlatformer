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
    public static event Action OnJumpInputStop;
    public static event Action OnJumpAltInput;
    public static event Action OnJumpAltInputStop;

    public static event Action<Magnet.Charge> OnMagnetGunSetCharge;

    const KeyCode MOVE_LEFT_KEY = KeyCode.A;
    const KeyCode MOVE_RIGHT_KEY = KeyCode.D;
    const KeyCode JUMP_KEY = KeyCode.Space;
    const KeyCode JUMP_KEY_ALT = KeyCode.W;
    const KeyCode TOGGLE_CHARGE_KEY = KeyCode.E;

    const KeyCode MOVE_LEFT_ARROW_KEY = KeyCode.LeftArrow;
    const KeyCode MOVE_RIGHT_ARROW_KEY = KeyCode.RightArrow;
    const KeyCode JUMP_ARROW_KEY = KeyCode.UpArrow;

    //Magnet.Charge _toggleCharge = Magnet.Charge.Positive;

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
        OnMagnetGunSetCharge?.Invoke(Magnet.Charge.Positive);
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

        if (Input.GetKey(MOVE_LEFT_KEY) || Input.GetKey(MOVE_LEFT_ARROW_KEY))
        {
            OnMoveLeftInput?.Invoke();
        }
        else OnMoveLeftInputStop?.Invoke();

        // Move Right

        if (Input.GetKey(MOVE_RIGHT_KEY) || Input.GetKey(MOVE_RIGHT_ARROW_KEY))
        {
            OnMoveRightInput?.Invoke();
        }
        else OnMoveRightInputStop?.Invoke();

        // Jump

        if (Input.GetKeyDown(JUMP_KEY) || Input.GetKeyDown(JUMP_ARROW_KEY))
        {
            OnJumpInput?.Invoke();
        }
        if (Input.GetKeyUp(JUMP_KEY) || Input.GetKeyUp(JUMP_ARROW_KEY))
        {
            OnJumpInputStop?.Invoke();
        }

        if (Input.GetKeyDown(JUMP_KEY_ALT))
        {
            OnJumpAltInput?.Invoke();
        }
        if (Input.GetKeyUp(JUMP_KEY_ALT))
        {
            OnJumpAltInputStop?.Invoke();
        }

        // Set charge with mosue click
        if (Input.GetMouseButtonDown(0))
        {
            OnMagnetGunSetCharge?.Invoke(Magnet.Charge.Positive);
        }
        if (Input.GetMouseButtonDown(1))
        {
            OnMagnetGunSetCharge?.Invoke(Magnet.Charge.Negative);
        }
    }
}