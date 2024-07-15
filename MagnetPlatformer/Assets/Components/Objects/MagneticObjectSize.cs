using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The centralizaed place to set the size of this object.
/// When the size changes, it updates:
/// - The size in Box Collider (Trigger) (in Parent)
/// - The size in Box Collider (Physics Collider)
/// - The size in Sprite Renderer
/// </summary>

public class MagneticObjectSize : MonoBehaviour
{
    public Vector2 Size = Vector2.one;
    [SerializeField] LayerMask physicsLayer;

    [ContextMenu("Update Component Sizes")]
    void UpdateComponentSizes()
    {
        GetComponent<BoxCollider2D>().size = Size;

        List<GameObject> listPhysics = new List<GameObject>();
        Method.GetChildRecursive_MatchesLayer(gameObject, listPhysics, physicsLayer);
        listPhysics[0].GetComponent<BoxCollider2D>().size = Size;

        List<GameObject> listSpriteRenderer = new List<GameObject>();
        Method.GetChildRecursive_ContainsSpriteRenderer(gameObject, listSpriteRenderer);
        listSpriteRenderer[0].GetComponent<SpriteRenderer>().size = Size;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(transform.position, Size);
    }
}
