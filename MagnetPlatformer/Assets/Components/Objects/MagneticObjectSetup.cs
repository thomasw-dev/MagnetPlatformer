using UnityEngine;

public class MagneticObjectSetup : MonoBehaviour
{
    public Vector2 Size = Vector2.one;
    public MagneticObject.Type Type;

    void OnValidate()
    {
        UpdateSize(Size);
        UpdateType(Type);
    }

    void UpdateSize(Vector2 size)
    {
        // Set the size of Box Collider (Trigger)

        GetComponent<BoxCollider2D>().size = size;

        // Get child "Physics Collider" and set the size

        GameObject child_PhysicsCollider = Method.GetChildrenMeetCondition(gameObject, IsInPhysicsLayer)[0];
        child_PhysicsCollider.GetComponent<BoxCollider2D>().size = size;

        bool IsInPhysicsLayer(GameObject gameObject)
        {
            return Method.IsInLayer(gameObject, Constants.LAYER.Physics.ToString());
        }

        // Get child "Sprite" and set the size

        GameObject child_SpriteRenderer = Method.GetChildrenMeetCondition(gameObject, HasEnabledSpriteRenderer)[0];
        child_SpriteRenderer.GetComponent<SpriteRenderer>().size = size;

        bool HasEnabledSpriteRenderer(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                return spriteRenderer.enabled;
            }
            else return false;
        }
    }

    void UpdateType(MagneticObject.Type type)
    {
        // Update Rigidbody

        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        if (type == MagneticObject.Type.Free) SetRigidbodyParameters(RigidbodyType2D.Dynamic, RigidbodyConstraints2D.FreezeRotation);
        if (type == MagneticObject.Type.Vertical) SetRigidbodyParameters(RigidbodyType2D.Dynamic, RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX);
        if (type == MagneticObject.Type.Horizontal) SetRigidbodyParameters(RigidbodyType2D.Dynamic, RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY);
        if (type == MagneticObject.Type.Static) SetRigidbodyParameters(RigidbodyType2D.Static, RigidbodyConstraints2D.FreezeAll);
        if (type == MagneticObject.Type.Patrol) SetRigidbodyParameters(RigidbodyType2D.Kinematic, RigidbodyConstraints2D.FreezeRotation);

        void SetRigidbodyParameters(RigidbodyType2D bodyType, RigidbodyConstraints2D constraints)
        {
            rigidbody.bodyType = bodyType;
            rigidbody.constraints = constraints;
        }

        // Update Patrol

        if (type == MagneticObject.Type.Patrol)
        {
            // Add Patrol object and component if not already having



            // Enable the Patrol object and component if exist



        }
        else
        {
            // Disable the Patrol object if exist



        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(transform.position, Size);
    }
}
