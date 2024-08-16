using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class CollisionKill : MonoBehaviour
{
    [Header("Assign Fields")]
    [SerializeField] Transform[] _castPoints = new Transform[4]; // 0 (Top), 1 (Bottom), 2 (Left), 3 (Right)

    [Header("Settings")]
    [SerializeField] float _castDistance = 0.05f;
    [SerializeField] LayerMask _includeLayers;

    [Header("Values")]
    public List<GameObject> HitDirection = new List<GameObject>(4); // 0 (Top), 1 (Bottom), 2 (Left), 3 (Right)

    public event Action OnKill;
    public event Action<Direction.Type, List<GameObject>> OnKillDirection;

    void FixedUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            DirectionBoxCast(i);
        }
        DetectVerticalCollisionKill();
        DetectHorizontalCollisionKill();
    }

    void DirectionBoxCast(int i)
    {
        RaycastHit2D hit = Physics2D.BoxCast(_castPoints[i].position, _castPoints[i].localScale, 0, IndexToDirection(i), _castDistance, _includeLayers);

        // Cache the hit object if any
        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != HitDirection[i])
            {
                HitDirection[i] = hitObject;
            }
        }
        // Remove the cached object that isn't currently hit by the cast
        else
        {
            HitDirection[i] = null;
        }
    }

    void DetectVerticalCollisionKill()
    {
        if (HitDirection[0] != null && HitDirection[1] != null)
        {
            OnKill?.Invoke();
            OnKillDirection?.Invoke(Direction.Type.Vertical, new List<GameObject> { HitDirection[0], HitDirection[1] });
        }
    }

    void DetectHorizontalCollisionKill()
    {
        if (HitDirection[2] != null && HitDirection[3] != null)
        {
            OnKill?.Invoke();
            OnKillDirection?.Invoke(Direction.Type.Horizontal, new List<GameObject> { HitDirection[2], HitDirection[3] });
        }
    }

    Vector2 IndexToDirection(int i)
    {
        return i switch
        {
            0 => Vector2.up,
            1 => Vector2.down,
            2 => Vector2.left,
            3 => Vector2.right,
            _ => Vector2.zero
        };
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < 4; i++)
        {
            Vector2 direction = IndexToDirection(i);
            Gizmos.DrawRay(_castPoints[i].position, direction * _castDistance);
        }
    }
}
