using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool IsGrounded { get => _isGrounded; }
    [SerializeField] bool _isGrounded;

    [SerializeField] List<Collider2D> _ignoreSelfColliders = new List<Collider2D>();

    List<Collider2D> _colliders = new List<Collider2D>();

    string[] _includeLayerNames = {
        Constants.LAYER.Physics.ToString(),
        Constants.LAYER.Environment.ToString()
    };

    void Update()
    {
        _colliders.Clear();

        ContactFilter2D contactFilter = new ContactFilter2D();
        Physics2D.OverlapCollider(GetComponent<Collider2D>(), contactFilter, _colliders);

        if (_colliders.Count == 0)
        {
            _isGrounded = false;
        }
        else
        {
            foreach (Collider2D collider in _colliders)
            {
                // Skip if it is a physics collider in the player itself
                bool skip = false;
                foreach (Collider2D selfCollider in _ignoreSelfColliders)
                {
                    skip = collider == selfCollider;
                }
                if (skip) continue;

                Debug.Log(collider.gameObject.name);
                string layerName = LayerMask.LayerToName(collider.gameObject.layer);
                if (Method.StringMatchesArrayElement(layerName, _includeLayerNames))
                {
                    _isGrounded = true;
                }
            }
        }
    }
}
