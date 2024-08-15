using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionKill : MonoBehaviour
{
    [SerializeField] Transform[] _castPoints = new Transform[4];
    [SerializeField] float _castDistance = 0.05f;
    [SerializeField] LayerMask _includeLayers;

    [Space(10)]

    [SerializeField] List<GameObject> _collisions;
    [SerializeField] GameObject _hitTop;
    [SerializeField] GameObject _hitBottom;
    [SerializeField] GameObject _hitLeft;
    [SerializeField] GameObject _hitRight;

    public event Action OnKill;
    public event Action<Direction.Type> OnKillDirection;

    void FixedUpdate()
    {
        List<GameObject> hitObjects = new List<GameObject>();
        GameObject hitTop = null;
        GameObject hitBottom = null;
        GameObject hitLeft = null;
        GameObject hitRight = null;

        for (int i = 0; i < 4; i++)
        {
            Vector2 direction = IndexToDirection(i);
            RaycastHit2D hit = Physics2D.BoxCast(_castPoints[i].position, _castPoints[i].localScale, 0, direction, _castDistance, _includeLayers);
            GameObject hitObject = hit.collider.gameObject;

            // Handle hit object if any
            if (hit.collider.gameObject != null)
            {
                _collisions.Add(hitObject);
                if (i == 0) _hitTop = hitObject;
                if (i == 1) _hitBottom = hitObject;
                if (i == 2) _hitLeft = hitObject;
                if (i == 3) _hitRight = hitObject;
            }

            // Check cached hit object
            if (i == 0)
            {
                _hitTop = hitObject;
            }
        }

        if (hitTop != null && _hitTop != hitTop && hitBottom != null && _hitBottom != hitBottom)
        {
            OnKill?.Invoke();
            OnKillDirection?.Invoke(Direction.Type.Vertical);
        }

        if (hitLeft != null && _hitLeft != hitLeft && hitRight != null && _hitRight != hitRight)
        {
            OnKill?.Invoke();
            OnKillDirection?.Invoke(Direction.Type.Horizontal);
        }
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
}
