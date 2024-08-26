using System;
using DG.Tweening;
using UnityEngine;

public class MagnetGunController : MonoBehaviour
{
    // State

    public enum StateEnum { Available, Cooldown, Refill }
    public StateController<StateEnum> StateController = new StateController<StateEnum>();
    [SerializeField] StateEnum _state; // Inspector

    // Values

    [HideInInspector] public MagnetGunValues Values;

    [Header("Dependencies")]
    // These fields are required to be assigned in order for this module to function.

    [Tooltip("The point where this magnet gun attaches its position to.")]
    [SerializeField] Transform _attachPoint;

    [Tooltip("The UI area where mouse click on it triggers the firing of this gun.")]
    [SerializeField] MagnetGunMouseArea _mouseArea;

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
    public event Action<Magnet.Charge> OnCurrentChargeChanged;

    // Fire

    public event Action<Magnet.Charge> OnFire;
    public event Action OnFireRelease;

    [Header("Shoot Ray")]

    [SerializeField] Transform _shootPoint;

    LayerMask _includeLayer;
    const float RAYCAST_LENGTH = 1000f;

    [Header("Cooldown & Refill")]

    [Range(0, MagnetGunValues.COOLDOWN_DURATION)]
    [SerializeField] float _cooldownBar;
    Tweener _cooldownTween;
    [Range(0, MagnetGunValues.AMMO_MAX)]
    [SerializeField] float _refillBar;
    Tweener _refillTween;

    // --------------------

    void Awake()
    {
        _includeLayer = LayerMask.GetMask(Constants.LAYER.Magnetic.ToString());
    }

    void OnValidate()
    {
        if (_mouseArea != null)
        {
            _mouseArea.OnMagnetSetCharge += SetCharge;
            _mouseArea.OnLeftButtonDown += Fire;
            _mouseArea.OnLeftButtonUp += FireRelease;
            _mouseArea.OnRightButtonDown += Fire;
            _mouseArea.OnRightButtonUp += FireRelease;
        }
    }

    void OnEnable()
    {
        GameState.Play.OnEnter += EnterPlay;
        //InputManager.OnMagnetGunSetCharge += SetCharge;
        GameState.Play.OnExit += ExitPlay;
        if (_mouseArea != null)
        {
            _mouseArea.OnMagnetSetCharge += SetCharge;
            _mouseArea.OnLeftButtonDown += Fire;
            _mouseArea.OnLeftButtonUp += FireRelease;
            _mouseArea.OnRightButtonDown += Fire;
            _mouseArea.OnRightButtonUp += FireRelease;
        }
    }

    void OnDisable()
    {
        GameState.Play.OnEnter -= EnterPlay;
        //InputManager.OnMagnetGunSetCharge -= SetCharge;
        GameState.Play.OnExit -= ExitPlay;

        if (_mouseArea != null)
        {
            _mouseArea.OnMagnetSetCharge -= SetCharge;
            _mouseArea.OnLeftButtonDown -= Fire;
            _mouseArea.OnLeftButtonUp -= FireRelease;
            _mouseArea.OnRightButtonDown -= Fire;
            _mouseArea.OnRightButtonUp -= FireRelease;
        }
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
        //_currentCharge = CurrentCharge;

        AttachSelfToPoint();
        AimSelfAtCursor();
    }

    void AttachSelfToPoint()
    {
        if (_attachPoint != null)
        {
            transform.position = _attachPoint.position;
        }
    }

    void AimSelfAtCursor()
    {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.Rotate(0f, 0f, -90f); // left side -> top side aiming
    }

    void SetCharge(Magnet.Charge charge)
    {
        CurrentCharge = charge;
    }

    void Fire()
    {
        bool successfulHit = ShootRay(CurrentCharge);

        OnFire?.Invoke(CurrentCharge);

        if (Values.CostAmmoOnMagneticOnly)
        {
            if (successfulHit)
            {
                CostAmmo();
            }
        }
        else
        {
            CostAmmo();
        }
    }

    void FireRelease() => OnFireRelease?.Invoke();

    bool ShootRay(Magnet.Charge charge)
    {
        Vector2 origin = _shootPoint == null ? transform.position : _shootPoint.position;
        Vector2 direction = transform.up;

        // Only shoot and detect on the Magnetic layer
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, RAYCAST_LENGTH, _includeLayer);

        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;

            if (Log.MagnetWeaponHit)
            {
                Debug.Log("Hit: " + hitObject.name);
            }

            if (hitObject.TryGetComponent(out MagneticInteractionController controller))
            {
                // Only trigger when the charges are different
                if (CurrentCharge != controller.CurrentCharge)
                {
                    controller.OnAlterCharge?.Invoke(charge);
                    return true;
                }
            }
        }
        return false;
    }

    void CostAmmo()
    {
        if (Values.Ammo > 0)
        {
            Values.Ammo--;
        }

        CooldownRefill();
    }

    void CooldownRefill()
    {
        // Kill any current cooldown/refill progress
        if (_refillTween != null && _refillTween.IsActive()) _refillTween.Kill();
        if (_cooldownTween != null && _cooldownTween.IsActive()) _cooldownTween.Kill();

        // Start the cooldown again
        CooldownTween();
    }

    void CooldownTween()
    {
        _cooldownTween = DOTween.To(x => _cooldownBar = x, 0, Values.CooldownDuration, Values.CooldownDuration)
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

    void ExitPlay() => SetCharge(Magnet.Charge.Neutral);
}
