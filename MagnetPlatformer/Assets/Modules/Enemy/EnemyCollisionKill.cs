using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionKill : MonoBehaviour
{
    [Header("Dependencies")] // Required to be assigned in the Inspector
    [SerializeField] RigidbodyCollision _rigidbodyCollision;

    [Header("Included Collisions")]
    [SerializeField] List<GameObject> _includedCollisions;

    const float BOXCAST_DISTANCE = 0.01f;

    LayerMask[] _includeLayers;

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
        _includedCollisions = new List<GameObject>();
        bool isHitTop = false;
        bool isHitBottom = false;

        foreach (LayerMask layerMask in _includeLayers)
        {
            RaycastHit2D hitTop = Physics2D.BoxCast(transform.position, _rigidbodyCollision.transform.localScale, 0, Vector2.up, BOXCAST_DISTANCE, layerMask);
            if (hitTop.collider != null)
            {
                _includedCollisions.Add(hitTop.collider.gameObject);
                isHitTop = true;
                Debug.Log("Top side hit: " + hitTop.collider.name);
            }
        }

        foreach (LayerMask layerMask in _includeLayers)
        {
            RaycastHit2D hitBottom = Physics2D.BoxCast(transform.position, _rigidbodyCollision.transform.localScale, 0, Vector2.down, BOXCAST_DISTANCE, layerMask);
            if (hitBottom.collider != null)
            {
                _includedCollisions.Add(hitBottom.collider.gameObject);
                isHitBottom = true;
                Debug.Log("Bottom side hit: " + hitBottom.collider.name);
            }
        }

        if (isHitTop && isHitBottom)
        {
            Debug.Log("Both top and bottom sides are collided!");
        }
    }
}
