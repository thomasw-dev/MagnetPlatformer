using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The centralizaed place to set the size of this object.
/// When the size changes, it updates:
/// - The size in Box Collider (Trigger) (in Parent)
/// - The size in Box Collider (Physics Collider)
/// - The size in Sprite Renderer
/// </summary>

public class MagneticObjectSetup : MonoBehaviour
{
    public Vector2 Size = Vector2.one;

    void OnValidate()
    {
        UpdateComponentSizes(Size);
    }

    void UpdateComponentSizes(Vector2 size)
    {
        GetComponent<BoxCollider2D>().size = size;

        // Get child "Physics Collider"
        GameObject child_PhysicsCollider = Method.GetChildrenMeetCondition(gameObject, IsInPhysicsLayer)[0];
        child_PhysicsCollider.GetComponent<BoxCollider2D>().size = size;

        // Get child "Sprite"
        GameObject child_SpriteRenderer = Method.GetChildrenMeetCondition(gameObject, HasEnabledSpriteRenderer)[0];
        child_SpriteRenderer.GetComponent<SpriteRenderer>().size = size;
    }

    bool IsInPhysicsLayer(GameObject gameObject)
    {
        return Method.IsInLayer(gameObject, Constants.LAYER.Physics.ToString());
    }

    bool HasEnabledSpriteRenderer(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            return spriteRenderer.enabled;
        }
        else return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(transform.position, Size);
    }
}
