using System;
using System.Collections.Generic;
using UnityEngine;

public class MagneticInteractionController : MonoBehaviour
{
    // State

    public enum StateEnum { Normal, AlteredCharge }
    public StateController<StateEnum> StateController = new StateController<StateEnum>();
    [SerializeField] StateEnum _state; // Inspector

    [Header("Dependencies")]
    // These fields are required to be assigned in order for this module to function.

    public MagneticInteractionValues Values;
    [SerializeField] Rigidbody2D _rigidbody;

    [Header("Interactions")]

    [Tooltip("The list of controllers reacting to the magnetic force emitted from this controller.")]
    public List<MagneticInteractionController> ReactingControllers = new();

    [Tooltip("The list of controllers emitting magnetic forces towards this controller.")]
    public List<MagneticInteractionController> EmittingControllers = new();

    public event Action<MagneticInteractionController> OnEmitMagneticForce;

    public List<ChargedForce> AppliedForces = new();
    [HideInInspector]
    public Vector2 NetAppliedForce
    {
        get
        {
            Vector2 netForce = Vector2.zero;
            foreach (ChargedForce force in AppliedForces)
            {
                netForce += force.Vector;
            }
            return netForce;
        }
    }

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

    public Action<Magnet.Charge> OnCurrentChargeChanged;

    // Alter Charge

    public Action<Magnet.Charge> OnAlterCharge;

    // --------------------

    bool DependenciesNullCheck()
    {
        bool pass = true;
        if (_rigidbody == null)
        {
            Debug.LogError($"Dependency missing: Rigidbody is not assigned.", this);
            pass = false;
        }
        if (Values == null)
        {
            Debug.LogError($"Dependency missing: Config is not assigned.", this);
            pass = false;
        }
        return pass;
    }

    void Awake()
    {
        DependenciesNullCheck();
    }

    void OnEnable()
    {
        OnAlterCharge += AlterCharge;
    }

    void OnDisable()
    {
        OnAlterCharge -= AlterCharge;

        // Remove this controller from the emitting controllers list in each reacting controller
        foreach (var reactingController in ReactingControllers.ToArray())
        {
            // Remove the applied force in the reacting controller at the index of the emitting controller being removed
            int index = reactingController.EmittingControllers.IndexOf(this);
            reactingController.AppliedForces.RemoveAt(index);

            // Unbind the relationship between these two controllers
            reactingController.EmittingControllers.Remove(this);
            ReactingControllers.Remove(reactingController);

            // Unsubscribe: make the reacting controller stop reacting to the magnetic force emitted from this controller
            OnEmitMagneticForce -= reactingController.ReactToMagneticForce;
        }
    }

    void Start()
    {
        StateController.ChangeState(StateEnum.Normal);
    }

    void Update()
    {
        UpdateInteractionControllers();
        _state = StateController.CurrentEnum;
    }

    void FixedUpdate()
    {
        OnEmitMagneticForce?.Invoke(this);
    }

    void UpdateInteractionControllers()
    {
        // The list of magnetic interaction controllers found by the circle radius check
        List<MagneticInteractionController> foundControllers = new();
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, Values.EmissionRadius, LayerMask.GetMask(Constants.LAYER.Magnetic.ToString()));
        foreach (var col in cols)
        {
            // If there is a found controller (not self, active gameObject and component enabled)
            if (col.gameObject != gameObject && col.gameObject.TryGetComponent(out MagneticInteractionController foundController) && foundController.isActiveAndEnabled)
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
                // Bind the relationship between these two controllers
                ReactingControllers.Add(foundController);
                foundController.EmittingControllers.Add(this);

                // Add an entry to the applied forces list in the found controller as it has a new emitting controller
                foundController.AppliedForces.Add(new ChargedForce());

                // Subscribe: make the found controller start reacting to the magnetic force emitted from this controller
                OnEmitMagneticForce += foundController.ReactToMagneticForce;
            }
        }

        // Check each reacting controller
        foreach (var reactingController in ReactingControllers.ToArray())
        {
            // If a reacting controller is not detected by the circle radius check (no longer in the area), or it is disabled
            if (!foundControllers.Contains(reactingController) || !reactingController.isActiveAndEnabled)
            {
                // Remove the applied force in the reacting controller at the index of the emitting controller being removed
                int index = reactingController.EmittingControllers.IndexOf(this);
                reactingController.AppliedForces.RemoveAt(index);

                // Unbind the relationship between these two controllers
                reactingController.EmittingControllers.Remove(this);
                ReactingControllers.Remove(reactingController);

                // Unsubscribe: make the reacting controller stop reacting to the magnetic force emitted from this controller
                OnEmitMagneticForce -= reactingController.ReactToMagneticForce;
            }
        }
    }

    void ReactToMagneticForce(MagneticInteractionController emittingController)
    {
        // Add force to rigidbody
        Vector2 magneticforce = MagneticInteractionPhysics.Calculate(transform.position, emittingController.transform.position, emittingController.Values.EmissionGain);
        ChargedForce chargedForce = MagneticInteractionPhysics.ConvertToChargedForce(magneticforce, CurrentCharge, emittingController.CurrentCharge);
        _rigidbody.AddForce(chargedForce.Vector);

        // Find the index of the emittingController in the list
        int index = EmittingControllers.IndexOf(emittingController);
        // Update the applied force value
        if (index != -1)
        {
            AppliedForces[index] = chargedForce;
        }
    }

    void AlterCharge(Magnet.Charge charge)
    {
        CurrentCharge = charge;
    }
}
