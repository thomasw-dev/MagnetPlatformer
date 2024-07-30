using System;
using DG.Tweening;
using UnityEngine;

public class MagnetGunController : MonoBehaviour
{
    public enum StateEnum { Available, Cooldown, Refill }
    public StateController<StateEnum> StateController = new StateController<StateEnum>();
    [SerializeField] StateEnum _state; // Inspector

    [Header("Dependencies")]
    // These fields are required to be assigned in order for this module to function.

    public MagnetGunValues Values;
    [SerializeField] Transform _attachPoint;
    [SerializeField] MagnetGunMouseArea _mouseArea;

    public static event Action<Magnet.Charge> OnFireWeapon;
    public static event Action OnFireWeaponStop;

    [Header("Charge")]
    [SerializeField] Magnet.Charge _currentCharge = Magnet.Charge.Positive;
    public Magnet.Charge CurrentCharge
    {
        get { return _currentCharge; }
        set
        {
            if (_currentCharge != value) { OnCurrentChargeChanged?.Invoke(value); }
            _currentCharge = value;
        }
    }
    public Action<Magnet.Charge> OnCurrentChargeChanged;

    [SerializeField] Sprite[] _magnetSprites;
    SpriteRenderer _spriteRenderer;

    [Header("Cooldown & Refill")]
    [Range(0, MagnetGunValues.COOLDOWN_DURATION)]
    [SerializeField] float _coolDownBar;
    Tweener _cooldownTween;
    [Range(0, MagnetGunValues.AMMO_MAX)]
    [SerializeField] float _refillBar;
    Tweener _refillTween;

    // --------------------

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        GameState.Play.OnEnter += EnterPlay;
        InputManager.OnMagnetGunSetCharge += InputSetCharge;
        _mouseArea.OnLeftButtonDown += FireWeapon;
        _mouseArea.OnLeftButtonUp += FireWeaponStop;
        _mouseArea.OnRightButtonDown += FireWeapon;
        _mouseArea.OnRightButtonUp += FireWeaponStop;
        MagnetGunVisual.OnAlterMagneticObjectCharge += CostAmmo;
        GameState.Play.OnExit += ExitPlay;
    }

    void OnDisable()
    {
        GameState.Play.OnEnter -= EnterPlay;
        InputManager.OnMagnetGunSetCharge -= InputSetCharge;
        _mouseArea.OnLeftButtonDown -= FireWeapon;
        _mouseArea.OnLeftButtonUp -= FireWeaponStop;
        _mouseArea.OnRightButtonDown -= FireWeapon;
        _mouseArea.OnRightButtonUp -= FireWeaponStop;
        MagnetGunVisual.OnAlterMagneticObjectCharge -= CostAmmo;
        GameState.Play.OnExit -= ExitPlay;
    }

    void EnterPlay()
    {
        SetCharge(CurrentCharge);
    }

    void Start()
    {
        StateController.ChangeState(StateEnum.Available);
    }

    void Update()
    {
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
        transform.position = _attachPoint.position;
    }

    void SetCharge(Magnet.Charge charge)
    {
        CurrentCharge = charge;
        SetMagnetSprite(charge);
    }

    void InputSetCharge(Magnet.Charge charge)
    {
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
        if (Values.Ammo <= 0) return;

        OnFireWeapon?.Invoke(CurrentCharge);

        if (!Values.CostAmmoOnMagneticOnly)
        {
            CostAmmo();
        }
    }

    void CostAmmo()
    {
        if (Values.Ammo > 0) Values.Ammo--;

        if (_refillTween != null && _refillTween.IsActive())
        {
            _refillTween.Kill();
        }

        if (_cooldownTween != null && _cooldownTween.IsActive())
        {
            _cooldownTween.Kill();
        }

        CooldownTween();
    }

    void CooldownTween()
    {
        _cooldownTween = DOTween.To(x => _coolDownBar = x, 0, Values.CooldownDuration, Values.CooldownDuration)
            .SetEase(Ease.Linear)
            .SetAutoKill(false)
            .OnPlay(() =>
            {
                StateController.ChangeState(StateEnum.Cooldown);
            })
            .OnUpdate(() =>
            {

            })
            .OnComplete(() =>
            {
                RefillTween();
            });

        _cooldownTween.Play();
    }

    void RefillTween()
    {
        _refillTween = DOTween.To(x => _refillBar = x, Values.Ammo, MagnetGunValues.AMMO_MAX, Values.RefillOneDuration * (MagnetGunValues.AMMO_MAX - Values.Ammo))
            .SetEase(Ease.Linear)
            .SetAutoKill(false)
            .OnPlay(() =>
            {
                StateController.ChangeState(StateEnum.Refill);
            })
            .OnUpdate(() =>
            {
                Values.Ammo = Mathf.FloorToInt(_refillBar);
            })
            .OnComplete(() =>
            {
                StateController.ChangeState(StateEnum.Available);
            });

        _refillTween.Play();
    }

    void FireWeaponStop() => OnFireWeaponStop?.Invoke();

    void ExitPlay() => SetCharge(Magnet.Charge.Neutral);
}
