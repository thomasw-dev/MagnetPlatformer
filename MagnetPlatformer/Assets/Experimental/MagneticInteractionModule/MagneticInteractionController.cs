using UnityEngine;

/// <summary>
/// Objects having this module will intreact with magnetic forces:
/// - Emit magnetic forces
/// - React to magnetic forces
/// </summary>

[RequireComponent(typeof(Collider2D))]
public class MagneticInteractionController : MonoBehaviour
{
    // These fields are required to be assigned in order for this module to function.
    [Header("Dependencies")]

    [SerializeField] Rigidbody2D _rigidbody;
    public MagneticInteractionConfig Config;

    void Awake()
    {
        // Dependencies null checks
        if (_rigidbody == null) Debug.LogError($"Dependency missing: Rigidbody is not assigned.", this);
        if (Config == null) Debug.LogError($"Dependency missing: Config is not assigned.", this);
    }

    void Reset()
    {
        gameObject.layer = LayerMask.NameToLayer(Constants.LAYER.Magnetic.ToString());
    }

    void Update()
    {
        if (Config.EmitForce)
        {
            EmitMagneticForce();
        }

        if (Config.ReactToForce)
        {
            ReactToMagneticForce();
        }

        if (Config.UseGravity)
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
        if (Config.EmissionRadius)
        {
            /*switch (CurrentCharge)
            {
                case Magnet.Charge.Neutral: return;
                case Magnet.Charge.Positive: Gizmos.color = Color.red; break;
                case Magnet.Charge.Negative: Gizmos.color = Color.blue; break;
            }*/
            Gizmos.DrawWireSphere(transform.position, Config.Radius);
        }
    }
}
