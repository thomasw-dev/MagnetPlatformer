using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class MagnetWeapon : MonoBehaviour
{
    public enum StateEnum
    {
        Available, Cooldown, Refill
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

    // Ammo, Cooldown, Refill

    [SerializeField] bool _costAmmoOnMagneticOnly = true;

    // Ammo
    const int AMMO_MAX = 6;

    [Range(0, AMMO_MAX)]
    public int Ammo = AMMO_MAX;

    // Cooldown
    const float COOLDOWN_DURATION = 1.0f;
    Tweener _cooldownTween;

    [Range(0, COOLDOWN_DURATION)]
    [SerializeField] float _cooldownDuration = 1.0f;

    [Range(0, COOLDOWN_DURATION)]
    [SerializeField] float _coolDownBar;

    // Refill
    const float REFILL_ONE_DURATION = 0.5f;
    Tweener _refillTween;

    [Range(0, REFILL_ONE_DURATION)]
    [SerializeField] float _refillOneDuration = REFILL_ONE_DURATION;

    [Range(0, AMMO_MAX)]
    [SerializeField] float _refillBar;

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
        MagnetWeaponAimRay.OnAlterMagneticObjectCharge += CostAmmo;
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
        MagnetWeaponAimRay.OnAlterMagneticObjectCharge -= CostAmmo;
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
        if (Ammo <= 0) return;

        OnFireWeapon?.Invoke(CurrentCharge);

        if (!_costAmmoOnMagneticOnly)
        {
            CostAmmo();
        }
    }

    void CostAmmo()
    {
        if (Ammo > 0) Ammo--;

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
        _cooldownTween = DOTween.To(x => _coolDownBar = x, 0, _cooldownDuration, _cooldownDuration)
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
        _refillTween = DOTween.To(x => _refillBar = x, Ammo, AMMO_MAX, _refillOneDuration * (AMMO_MAX - Ammo))
            .SetEase(Ease.Linear)
            .SetAutoKill(false)
            .OnPlay(() =>
            {
                StateController.ChangeState(StateEnum.Refill);
            })
            .OnUpdate(() =>
            {
                Ammo = Mathf.FloorToInt(_refillBar);
            })
            .OnComplete(() =>
            {
                StateController.ChangeState(StateEnum.Available);
            });

        _refillTween.Play();
    }

    void FireWeaponStop() => OnFireWeaponStop?.Invoke();

    void Restore() => SetCharge(Magnet.Charge.Neutral);
}
