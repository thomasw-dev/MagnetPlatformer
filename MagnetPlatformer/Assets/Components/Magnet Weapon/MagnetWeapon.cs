using System;
using System.Collections;
using UnityEngine;

public class MagnetWeapon : MonoBehaviour
{
    public enum StateEnum
    {
        Available, Cooldown, Reload
    }
    public StateController<StateEnum> StateController = new StateController<StateEnum>();
    [SerializeField] StateEnum _state; // Inspector

    [SerializeField] Transform _attachTo;

    public static event Action<Magnet.Charge> OnFireWeapon;
    public static event Action OnFireWeaponStop;

    public static Magnet.Charge CurrentCharge = Magnet.Charge.Neutral;
    [SerializeField] Magnet.Charge _currentCharge; // Inspector

    [SerializeField] Sprite[] _magnetSprites;
    SpriteRenderer _spriteRenderer;

    bool _isInputEnabled = true;

    // Reload Cooldown
    const int AMMO = 6;
    [Range(0, AMMO)]
    public int Ammo = AMMO;
    const float COOLDOWN_DURATION = 1.0f;
    [Range(0, COOLDOWN_DURATION)]
    [SerializeField] float _cooldownDuration = 0.25f;
    const float RELOAD_DURATION = 2.0f;
    [Range(0, RELOAD_DURATION)]
    [SerializeField] float _reloadDuration = RELOAD_DURATION;
    Magnet.Charge _prevCharge;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        InputManager.OnMagnetWeaponSetCharge += InputSetCharge;
        MagnetMouseControl.OnLeftButtonDown += FireWeapon;
        MagnetMouseControl.OnLeftButtonUp += FireWeaponStop;
        MagnetMouseControl.OnRightButtonDown += FireWeapon;
        MagnetMouseControl.OnRightButtonUp += FireWeaponStop;
        GameState.Play.OnEnter += EnterPlay;
        GameState.Play.OnExit += Restore;
    }

    void OnDisable()
    {
        InputManager.OnMagnetWeaponSetCharge -= InputSetCharge;
        MagnetMouseControl.OnLeftButtonDown -= FireWeapon;
        MagnetMouseControl.OnLeftButtonUp -= FireWeaponStop;
        MagnetMouseControl.OnRightButtonDown -= FireWeapon;
        MagnetMouseControl.OnRightButtonUp -= FireWeaponStop;
        GameState.Play.OnEnter -= EnterPlay;
        GameState.Play.OnExit -= Restore;
    }

    void EnterPlay()
    {
        SetCharge(Magnet.Charge.Positive);
    }

    void Start()
    {
        StateController.ChangeState(StateEnum.Available);
    }

    void Update()
    {
        // _isInputEnabled is only true when GameState is Playing
        _isInputEnabled = GameState.CurrentState == GameState.Play ? true : false;

        _state = StateController.CurrentEnum;
        _currentCharge = CurrentCharge;

        AimSelfAtCursor();
        AttachSelfToPlayer();
    }

    void AimSelfAtCursor()
    {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.Rotate(0f, 0f, -90f); // left side -> top side aiming
    }

    void AttachSelfToPlayer()
    {
        transform.position = _attachTo.position;
    }

    void SetCharge(Magnet.Charge charge)
    {
        CurrentCharge = charge;
        SetMagnetSprite(charge);
    }

    void InputSetCharge(Magnet.Charge charge)
    {
        if (!_isInputEnabled) { return; }
        CurrentCharge = charge;
        SetMagnetSprite(charge);
    }

    void SetMagnetSprite(Magnet.Charge charge)
    {
        _spriteRenderer.sprite = GetMagnetSpriteByCharge(charge);
        Sprite GetMagnetSpriteByCharge(Magnet.Charge charge) => charge switch
        {
            Magnet.Charge.Neutral => _magnetSprites[0],
            Magnet.Charge.Positive => _magnetSprites[1],
            Magnet.Charge.Negative => _magnetSprites[2],
            _ => _magnetSprites[0]
        };
    }

    void FireWeapon()
    {
        if (CurrentCharge == Magnet.Charge.Neutral) return;

        if (StateController.CurrentEnum != StateEnum.Available) return;

        Ammo--;
        if (Ammo == 0)
        {
            StartCoroutine(Reload());
        }
        else
        {
            StartCoroutine(Cooldown());
        }

        OnFireWeapon?.Invoke(CurrentCharge);
    }

    IEnumerator Reload()
    {
        StateController.ChangeState(StateEnum.Reload);
        _prevCharge = CurrentCharge;
        CurrentCharge = Magnet.Charge.Neutral;

        yield return new WaitForSeconds(RELOAD_DURATION);

        Ammo = AMMO;
        CurrentCharge = _prevCharge;
        StateController.ChangeState(StateEnum.Available);
    }

    IEnumerator Cooldown()
    {
        StateController.ChangeState(StateEnum.Cooldown);
        _prevCharge = CurrentCharge;
        CurrentCharge = Magnet.Charge.Neutral;

        yield return new WaitForSeconds(COOLDOWN_DURATION);

        CurrentCharge = _prevCharge;
        StateController.ChangeState(StateEnum.Available);
    }

    void FireWeaponStop() => OnFireWeaponStop?.Invoke();

    void Restore() => SetCharge(Magnet.Charge.Neutral);
}
