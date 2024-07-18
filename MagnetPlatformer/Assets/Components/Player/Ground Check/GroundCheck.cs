using UnityEngine;
using System;

public class GroundCheck : MonoBehaviour
{
    LayerMask _groundLayer;
    const float RADIUS = 0.5f;

    void Start()
    {
        _groundLayer = LayerMask.GetMask(
            Constants.LAYER.Physics.ToString(),
            Constants.LAYER.Environment.ToString()
        );
    }

    public bool IsGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, RADIUS, _groundLayer);
        return collider == null ? false : true;
    }
}