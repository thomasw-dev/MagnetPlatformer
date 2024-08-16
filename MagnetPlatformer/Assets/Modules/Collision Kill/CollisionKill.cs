using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionKill : MonoBehaviour
{
    [Header("Assign Fields")]
    [SerializeField] Transform[] _castPoints = new Transform[4]; // 0 (Top), 1 (Bottom), 2 (Left), 3 (Right)

    [Header("Settings")]
    [SerializeField] float _castDistance = 0.05f;
    [SerializeField] LayerMask _includeLayers;

    [Header("Values")]
    public List<GameObject> CollidingObjects = new List<GameObject>(4);
    public List<GameObject> LastVerticalKillObjects = new List<GameObject>(2);
    public List<GameObject> LastHorizontalKillObjects = new List<GameObject>(2);

    public event Action OnKill;
    public event Action<Direction.Type, List<GameObject>> OnKillDirection;
    public event Action<Direction.Type> OnClearTriggeredList;

    void FixedUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            DirectionBoxCast(i);
        }
        CheckVerticalKill();
        CheckHorizontalKill();
    }

    void DirectionBoxCast(int i)
    {
        RaycastHit2D hit = Physics2D.BoxCast(_castPoints[i].position, _castPoints[i].localScale, 0, Method.SideToVector2((Direction.Side)i), _castDistance, _includeLayers);

        // The BoxCast of this direction detects a collider, cache its GameObject
        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != CollidingObjects[i])
            {
                CollidingObjects[i] = hitObject;
            }
        }
        // The BoxCast of this direction doesn't detect a collider, remove the cached GameObject
        else
        {
            CollidingObjects[i] = null;
            ClearKillObjectsByDirection(i);
        }
    }

    void CheckVerticalKill()
    {
        if (CollidingObjects[(int)Direction.Side.Top] != null && CollidingObjects[(int)Direction.Side.Bottom] != null)
        {
            OnKill?.Invoke();

            LastVerticalKillObjects.Clear();
            LastVerticalKillObjects.Add(CollidingObjects[(int)Direction.Side.Top]);
            LastVerticalKillObjects.Add(CollidingObjects[(int)Direction.Side.Bottom]);

            OnKillDirection?.Invoke(Direction.Type.Vertical, LastVerticalKillObjects);
        }
    }

    void CheckHorizontalKill()
    {
        if (CollidingObjects[(int)Direction.Side.Left] != null && CollidingObjects[(int)Direction.Side.Right] != null)
        {
            OnKill?.Invoke();

            LastHorizontalKillObjects.Clear();
            LastHorizontalKillObjects.Add(CollidingObjects[(int)Direction.Side.Left]);
            LastHorizontalKillObjects.Add(CollidingObjects[(int)Direction.Side.Right]);

            OnKillDirection?.Invoke(Direction.Type.Horizontal, LastHorizontalKillObjects);
        }
    }

    void ClearKillObjectsByDirection(int i)
    {
        if (i == 0 || i == 1)
        {
            LastVerticalKillObjects.Clear();
            OnClearTriggeredList?.Invoke(Direction.Type.Vertical);
        }
        if (i == 2 || i == 3)
        {
            LastHorizontalKillObjects.Clear();
            OnClearTriggeredList?.Invoke(Direction.Type.Horizontal);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < 4; i++)
        {
            Vector2 direction = Method.SideToVector2((Direction.Side)i);
            Gizmos.DrawRay(_castPoints[i].position, direction * _castDistance);
        }
    }
}
