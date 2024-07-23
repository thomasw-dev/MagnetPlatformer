using System;
using UnityEngine;

/// <summary>
/// Objects having this module will intreact with magnetic forces:
/// - Emit magnetic forces
/// - React to magnetic forces
/// - Fall down by gravity
/// </summary>

[RequireComponent(typeof(Collider2D))]
public class MagneticInteractionController : MonoBehaviour
{
    // State
    public enum StateEnum
    {
        Normal, AlteredCharge
    }
    public StateController<StateEnum> StateController = new StateController<StateEnum>();
    [SerializeField] StateEnum _state; // Inspector

    // These fields are required to be assigned in order for this module to function.
    [Header("Dependencies")]

    [SerializeField] Rigidbody2D _rigidbody;
    public MagneticInteractionValues Values;

    // Charge
    public Magnet.Charge CurrentCharge
    {
        get { return _currentCharge; }
        set
        {
            if (_currentCharge != value) { OnCurrentChargeChanged?.Invoke(value); }
            _currentCharge = value;
        }
    }
    Magnet.Charge _currentCharge;
    Magnet.Charge _initialCharge;
    public event Action<Magnet.Charge> OnCurrentChargeChanged;

    void Awake()
    {
        // Dependencies null checks
        if (_rigidbody == null) Debug.LogError($"Dependency missing: Rigidbody is not assigned.", this);
        if (Values == null) Debug.LogError($"Dependency missing: Config is not assigned.", this);
    }

    void Reset()
    {
        gameObject.layer = LayerMask.NameToLayer(Constants.LAYER.Magnetic.ToString());
    }

    void Update()
    {
        if (Values.EmitForce)
        {
            EmitMagneticForce();
        }

        if (Values.UseGravity)
        {
            Vector2 gravityForce = MagneticForce.Calculate(_rigidbody.velocity, Vector2.down, 1f);
            _rigidbody.AddForce(gravityForce);
        }

        _state = StateController.CurrentEnum;
    }

    void EmitMagneticForce()
    {
        // Find magnetic objects within circle radius
    }
}
