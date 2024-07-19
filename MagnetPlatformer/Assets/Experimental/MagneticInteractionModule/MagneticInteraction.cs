using UnityEngine;

/// <summary>
/// Objects having this module will intreact with magnetic forces:
/// - Emit magnetic forces
/// - React to magnetic forces
/// </summary>

[RequireComponent(typeof(Collider2D))]
public class MagneticInteraction : MonoBehaviour
{
    // These fields are required to be assigned in order for this module to function.
    [Header("Dependencies")]

    [SerializeField] Rigidbody2D _rigidbody;

    [Header("Emission")]

    public bool EmitForce = true;

    [Range(0f, 1000f)]
    public float Force = 1f;

    [Range(0f, 50f)]
    public float Radius = 10f;

    [Header("Reaction")]

    public bool ReactToForce = true;

    [Header("Others")]

    public bool UseGravity = true;

    [Header("Gizmos")]

    public bool EmissionRadius = true;

    void Awake()
    {
        // Dependencies null checks
        if (_rigidbody == null) Debug.LogError($"Dependency missing: Rigidbody is not assigned.", this);
    }

    void Reset()
    {
        gameObject.layer = LayerMask.NameToLayer(Constants.LAYER.Magnetic.ToString());
    }

    void Update()
    {
        if (EmitForce)
        {
            EmitMagneticForce();
        }

        if (ReactToForce)
        {
            ReactToMagneticForce();
        }

        if (UseGravity)
        {
            //_rigidbody.AddForce(GravityForce(_rigidbody.mass));
        }
    }

    void EmitMagneticForce()
    {
        // Find magnetic objects within circle radius
    }

    void ReactToMagneticForce()
    {

    }

    void GravityForce(float mass)
    {
        Vector2 gravityForce = MagneticForce.Calculate(_rigidbody.velocity, Vector2.down, 1f) * _rigidbody.mass;
        _rigidbody.AddForce(gravityForce);
    }

    void OnDrawGizmosSelected()
    {
        if (EmissionRadius)
        {
            /*switch (CurrentCharge)
            {
                case Magnet.Charge.Neutral: return;
                case Magnet.Charge.Positive: Gizmos.color = Color.red; break;
                case Magnet.Charge.Negative: Gizmos.color = Color.blue; break;
            }*/
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
    }
}
