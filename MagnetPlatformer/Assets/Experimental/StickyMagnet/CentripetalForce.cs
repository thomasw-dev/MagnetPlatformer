using UnityEngine;

public class CentripetalForce : MonoBehaviour
{
    public Transform targetObject; // The object towards which the target object is attracted
    public float distanceMultiplier = 0.1f; // Multiplier for force based on distance
    [SerializeField] float _radius = 10f;

    Rigidbody2D rb;

    void Start()
    {
        rb = targetObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector3 direction = transform.position - targetObject.position;
        float distance = direction.magnitude;

        if (distance <= _radius)
        {
            rb.GetComponent<RigidbodyMoveKeys>().enabled = false;

            float forceMagnitude = 1 / distance * Mathf.Pow(distanceMultiplier, 6);
            Vector3 force = direction.normalized * forceMagnitude;

            rb.AddForce(force, ForceMode2D.Force);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}