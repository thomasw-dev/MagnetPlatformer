using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

/// <summary>
/// Objects having this module will intreact with magnetic forces:
/// - Emit magnetic forces
/// - React to magnetic forces
/// - Fall down by gravity
/// </summary>

[RequireComponent(typeof(Collider2D))]
public class MagneticInteractionController : MonoBehaviour
{
    // State --------

    public enum StateEnum { Normal, AlteredCharge }
    public StateController<StateEnum> StateController = new StateController<StateEnum>();
    [SerializeField] StateEnum _state; // Inspector

    // These fields are required to be assigned in order for this module to function.
    [Header("Dependencies")]

    [SerializeField] Rigidbody2D _rigidbody;
    public MagneticInteractionValues Values;

    // Charge --------

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

    // Forces --------

    // The list of controllers it is emitting magnetic force to
    List<MagneticInteractionController> _magneticInteractionControllers = new();
    List<Vector2> Forces = new List<Vector2>();
    Vector2 NetForce;

    public event Action<Vector2, Magnet.Charge> OnEmitMagneticForce;

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
        // Find all magnetic interaction controllers found within circle diameter, update the existing list to them and sub/unsubscribe to their Actions

        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, Values.Radius * 2, LayerMask.GetMask(Constants.LAYER.Magnetic.ToString()));
        foreach (var col in cols)
        {
            // If it is an active gameObject and has an enabled MagneticInteractionController
            if (col.gameObject.TryGetComponent(out MagneticInteractionController foundController) && foundController.isActiveAndEnabled)
            {
                // If it is not in the list, add it and make it subscribe to react to the force
                if (!_magneticInteractionControllers.Contains(foundController))
                {
                    _magneticInteractionControllers.Add(foundController);
                    OnEmitMagneticForce += foundController.ReactToMagneticForce;
                }
            }
        }

        // Calculate the force for each magnetic object in effect

    }

    void ReactToMagneticForce(Vector2 force, Magnet.Charge charge)
    {
        // Calculate the force for each magnetic object in effect
    }
}
