using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionKill : MonoBehaviour
{
    [SerializeField] Vector2 _boxCastSize = Vector2.one;
    [SerializeField] List<GameObject> _collisions;

    const float BOXCAST_DISTANCE = 0.01f;

    LayerMask[] _includeLayers;

    public event Action OnKill;
    bool _invoked = false;

    void Awake()
    {
        _includeLayers = new LayerMask[]
        {
            LayerMask.GetMask(Constants.LAYER.Environment.ToString()),
            LayerMask.GetMask(Constants.LAYER.Magnetic.ToString())
        };
    }

    void Update()
    {
        _collisions = new List<GameObject>();
        bool isHitTop = false;
        bool isHitBottom = false;
        bool isHitLeft = false;
        bool isHitRight = false;

        // Top
        foreach (LayerMask layerMask in _includeLayers)
        {
            RaycastHit2D hitTop = Physics2D.BoxCast(transform.position, _boxCastSize, 0, Vector2.up, BOXCAST_DISTANCE, layerMask);
            if (hitTop.collider != null)
            {
                _collisions.Add(hitTop.collider.gameObject);
                isHitTop = true;
            }
        }

        // Bottom
        foreach (LayerMask layerMask in _includeLayers)
        {
            RaycastHit2D hitBottom = Physics2D.BoxCast(transform.position, _boxCastSize, 0, Vector2.down, BOXCAST_DISTANCE, layerMask);
            if (hitBottom.collider != null)
            {
                _collisions.Add(hitBottom.collider.gameObject);
                isHitBottom = true;
            }
        }

        // Left
        foreach (LayerMask layerMask in _includeLayers)
        {
            RaycastHit2D hitLeft = Physics2D.BoxCast(transform.position, _boxCastSize, 0, Vector2.left, BOXCAST_DISTANCE, layerMask);
            if (hitLeft.collider != null)
            {
                _collisions.Add(hitLeft.collider.gameObject);
                isHitLeft = true;
            }
        }

        // Right
        foreach (LayerMask layerMask in _includeLayers)
        {
            RaycastHit2D hitRight = Physics2D.BoxCast(transform.position, _boxCastSize, 0, Vector2.right, BOXCAST_DISTANCE, layerMask);
            if (hitRight.collider != null)
            {
                _collisions.Add(hitRight.collider.gameObject);
                isHitRight = true;
            }
        }

        if ((isHitTop && isHitBottom) || (isHitLeft && isHitRight))
        {
            if (!_invoked) { OnKill?.Invoke(); }
        }
    }
}
