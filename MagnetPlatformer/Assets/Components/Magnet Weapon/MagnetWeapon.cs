using System;
using System.Collections;
using UnityEngine;

public class MagnetWeapon : MonoBehaviour
{
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
    const float RELOAD_DURATION = 2.0f;
    [SerializeField] int _ammo = AMMO;
    [SerializeField] bool _canFireWeapon = true;
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

    void Update()
    {
        // _isInputEnabled is only true when GameState is Playing
        _isInputEnabled = GameState.CurrentState == GameState.Play ? true : false;

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

        if (!_canFireWeapon) return;

        _ammo--;
        if (_ammo == 0)
        {
            StartCoroutine(Reload());
        }

        OnFireWeapon?.Invoke(CurrentCharge);
    }

    IEnumerator Reload()
    {
        Debug.Log("Enter cooldown");
        _canFireWeapon = false;
        _prevCharge = CurrentCharge;
        CurrentCharge = Magnet.Charge.Neutral;

        yield return new WaitForSeconds(RELOAD_DURATION);

        _ammo = AMMO;
        CurrentCharge = _prevCharge;
        _canFireWeapon = true;
        Debug.Log("Exit cooldown");
    }

    void FireWeaponStop() => OnFireWeaponStop?.Invoke();

    void Restore() => SetCharge(Magnet.Charge.Neutral);
}
