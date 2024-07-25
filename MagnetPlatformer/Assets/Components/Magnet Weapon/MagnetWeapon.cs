using System;
using System.Collections;
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

    const int AMMO_MAX = 6;
    [Range(0, AMMO_MAX)]
    public int Ammo = AMMO_MAX;

    const float COOLDOWN_DURATION = 1.0f;
    [Range(0, COOLDOWN_DURATION)]
    [SerializeField] float _cooldownDuration = 1.0f;
    [SerializeField] bool _isCoolingDown = false;

    const float REFILL_ONE_DURATION = 0.5f;
    [Range(0, REFILL_ONE_DURATION)]
    [SerializeField] float _refillOneDuration = REFILL_ONE_DURATION;
    [SerializeField] bool _isRefilling = false;

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
        MagnetWeaponAimRay.OnHitMagneticObject += CostAmmo;
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
        MagnetWeaponAimRay.OnHitMagneticObject -= CostAmmo;
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
        if (Ammo == 0) return;

        OnFireWeapon?.Invoke(CurrentCharge);

        if (!_costAmmoOnMagneticOnly)
        {
            CostAmmo();
        }
    }

    void CostAmmo()
    {
        if (!_costAmmoOnMagneticOnly) { return; }

        if (Ammo > 0) Ammo--;

        if (_isCoolingDown) StopCoroutine(Cooldown());
        if (_isRefilling) StopCoroutine(Refill());
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        Debug.Log("Start Cooldown");
        _isCoolingDown = true;
        StateController.ChangeState(StateEnum.Cooldown);
        yield return new WaitForSeconds(COOLDOWN_DURATION);
        StartCoroutine(Refill());
        _isCoolingDown = false;
    }

    IEnumerator Refill()
    {
        Debug.Log("Start Refill");
        _isRefilling = true;
        StateController.ChangeState(StateEnum.Refill);
        yield return new WaitForSeconds(REFILL_ONE_DURATION);
        Ammo++;
        Ammo = Mathf.Clamp(Ammo, 0, AMMO_MAX);
        Debug.Log("Add Ammo");
        if (Ammo < AMMO_MAX)
        {
            yield return StartCoroutine(Refill());
        }
        else
        {
            _isRefilling = false;
            yield break;
        }
    }

    void FireWeaponStop() => OnFireWeaponStop?.Invoke();

    void Restore() => SetCharge(Magnet.Charge.Neutral);
}
