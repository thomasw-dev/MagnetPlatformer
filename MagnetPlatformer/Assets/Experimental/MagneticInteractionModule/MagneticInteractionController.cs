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
    // These fields are required to be assigned in order for this module to function.
    [Header("Dependencies")]

    [SerializeField] Rigidbody2D _rigidbody;
    public MagneticInteractionValues Values;

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

        if (Values.ReactToForce)
        {
            ReactToMagneticForce();
        }

        if (Values.UseGravity)
        {
            Vector2 gravityForce = MagneticForce.Calculate(_rigidbody.velocity, Vector2.down, 1f);
            _rigidbody.AddForce(gravityForce);
        }
    }

    void EmitMagneticForce()
    {
        // Find magnetic objects within circle radius
    }

    void ReactToMagneticForce()
    {

    }
}
