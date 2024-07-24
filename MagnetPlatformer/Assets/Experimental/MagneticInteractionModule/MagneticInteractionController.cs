using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MagneticInteractionController : MonoBehaviour
{
    // State

    public enum StateEnum { Normal, AlteredCharge }
    public StateController<StateEnum> StateController = new StateController<StateEnum>();
    [SerializeField] StateEnum _state; // Inspector

    [Header("Dependencies")]
    // These fields are required to be assigned in order for this module to function.

    [SerializeField] Rigidbody2D _rigidbody;
    public MagneticInteractionValues Values;

    [Header("Interactions")]

    [Tooltip("The list of controllers reacting to the magnetic force emitted from this controller.")]
    public List<MagneticInteractionController> ReactingControllers = new();

    [Tooltip("The list of controllers emitting magnetic forces towards this controller.")]
    public List<MagneticInteractionController> EmittingControllers = new();

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

    // Reaction Forces

    [HideInInspector] public List<ChargedForce> ReactionForces = new();
    [HideInInspector] public Vector2 NetReactionForce;

    public event Action<MagneticInteractionController> OnEmitMagneticForce;

    // --------------------

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

    void Start()
    {
        StateController.ChangeState(StateEnum.Normal);
    }

    void Update()
    {
        UpdateReactingControllers();
        UpdateNetReactionForce();
        _state = StateController.CurrentEnum;
    }

    void FixedUpdate()
    {
        //ReactionForces.Clear();

        if (Values.EmitForce)
        {
            OnEmitMagneticForce?.Invoke(this);
        }
    }

    void UpdateReactingControllers()
    {
        // The list of magnetic interaction controllers found by the circle radius check
        List<MagneticInteractionController> foundControllers = new();
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, Values.EmissionRadius * 2, LayerMask.GetMask(Constants.LAYER.Magnetic.ToString()));
        foreach (var col in cols)
        {
            // If there is a found controller (active gameObject and component enabled)
            if (col.gameObject.TryGetComponent(out MagneticInteractionController foundController) && foundController.isActiveAndEnabled)
            {
                foundControllers.Add(foundController);
            }
        }

        // Check each found controller
        foreach (var foundController in foundControllers)
        {
            // If a found controller is not in the list of reacting controllers
            if (!ReactingControllers.Contains(foundController))
            {
                ReactingControllers.Add(foundController);
                // Make the found controller start reacting to the magnetic force emitted from this controller
                OnEmitMagneticForce += foundController.ReactToMagneticForce;
            }
        }

        // Check each reacting controller
        foreach (var reactingController in ReactingControllers.ToArray())
        {
            // If a reacting controller is not detected by the circle radius check (no longer in the area)
            if (!foundControllers.Contains(reactingController))
            {
                ReactingControllers.Remove(reactingController);
                // Make the reacting controller stop reacting to the magnetic force emitted from this controller
                OnEmitMagneticForce -= reactingController.ReactToMagneticForce;
            }
        }
    }

    void ReactToMagneticForce(MagneticInteractionController emittingController)
    {
        if (!EmittingControllers.Contains(emittingController))
        {
            EmittingControllers.Add(emittingController);
        }

        // Add force to rigidbody
        Vector2 distance = transform.position - emittingController.transform.position;
        Vector2 magneticforce = MagneticForce.Calculate(_rigidbody.velocity, distance, emittingController.Values.EmissionForce);
        ChargedForce chargedForce = MagneticForce.ConvertToChargedForce(magneticforce, CurrentCharge, emittingController.CurrentCharge);
        _rigidbody.AddForce(chargedForce.Vector);

        // Add the force to the list of reaction forces
        ReactionForces.Add(chargedForce);
    }

    void UpdateNetReactionForce()
    {
        Vector2 netForce = Vector2.zero;
        foreach (ChargedForce force in ReactionForces)
        {
            netForce += force.Vector;
        }
        NetReactionForce = netForce;
    }
}
