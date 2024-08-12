using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CollisionKill : MonoBehaviour
{
    [SerializeField] Transform[] _castPoints = new Transform[4];
    [SerializeField] float _castDistance = 0.05f;
    [SerializeField] LayerMask _includeLayers;

    [Space(10)]

    [SerializeField] List<GameObject> _collisions;

    [SerializeField] bool _invoked = false;
    [SerializeField] float _invokeResetDelay = 1f;
    Tweener _invokeResetTween;
    float _invokeResetTweenProgress;

    public event Action OnKill;
    public event Action<Direction.Type> OnKillDirection;

    void FixedUpdate()
    {
        _collisions = new List<GameObject>();
        bool isHitTop = false;
        bool isHitBottom = false;
        bool isHitLeft = false;
        bool isHitRight = false;

        for (int i = 0; i < 4; i++)
        {
            Vector2 direction = IndexToDirection(i);
            RaycastHit2D hit = Physics2D.BoxCast(_castPoints[i].position, _castPoints[i].localScale, 0, direction, _castDistance, _includeLayers);
            if (hit.collider != null)
            {
                _collisions.Add(hit.collider.gameObject);
                if (i == 0) isHitTop = true;
                if (i == 1) isHitBottom = true;
                if (i == 2) isHitLeft = true;
                if (i == 3) isHitRight = true;
            }
        }

        if (isHitTop && isHitBottom)
        {
            if (!_invoked)
            {
                OnKill?.Invoke();
                OnKillDirection?.Invoke(Direction.Type.Vertical);
                _invoked = true;
                StartInvokeResetTween(_invokeResetDelay);
            }
        }

        if (isHitLeft && isHitRight)
        {
            if (!_invoked)
            {
                OnKill?.Invoke();
                OnKillDirection?.Invoke(Direction.Type.Horizontal);
                _invoked = true;
                StartInvokeResetTween(_invokeResetDelay);
            }
        }
    }

    void StartInvokeResetTween(float delay)
    {
        // Kill any current tween progress
        if (_invokeResetTween != null && _invokeResetTween.IsActive()) _invokeResetTween.Kill();

        // Start the tween again
        _invokeResetTween = DOTween.To(x => _invokeResetTweenProgress = x, delay, 0, delay).SetEase(Ease.Linear)
            .SetAutoKill(false)
            .OnPlay(() =>
            {

            })
            .OnUpdate(() =>
            {

            })
            .OnComplete(() =>
            {
                _invoked = false;
            });

        _invokeResetTween.Play();
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
