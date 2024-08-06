using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool IsGrounded { get => _isGrounded; }

    [Tooltip("These fields need their values to be assigned in the Inspector.")]
    [Header("Assign Fields")]
    [SerializeField] List<Collider2D> _ignorePhysicsCollidersInSelf = new List<Collider2D>();

    [Space(10)]

    [SerializeField] bool _isGrounded;
    [SerializeField] List<Collider2D> _groundColliders = new List<Collider2D>();

    string[] _includeLayerNames =
    {
        Constants.LAYER.Physics.ToString(),
        Constants.LAYER.Environment.ToString()
    };

    void Update()
    {
        _isGrounded = _groundColliders.Count > 0;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        // Skip if it is a physics collider in the player itself
        if (_ignorePhysicsCollidersInSelf.Contains(collider)) return;

        string layerName = LayerMask.LayerToName(collider.gameObject.layer);
        if (Method.StringMatchesArrayElement(layerName, _includeLayerNames))
        {
            if (!_groundColliders.Contains(collider))
            {
                _groundColliders.Add(collider);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (_groundColliders.Contains(collider))
        {
            _groundColliders.Remove(collider);
        }
    }
}
