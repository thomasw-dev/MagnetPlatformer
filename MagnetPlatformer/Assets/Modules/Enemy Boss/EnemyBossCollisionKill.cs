using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyBossCollisionKill : MonoBehaviour
{
    [Header("Dependencies")] // Required to be assigned in the Inspector
    [SerializeField] Transform _rootParent;

    [SerializeField] float _squeezeDuration = 1f;

    EnemyController _enemyController;
    EnemyBossHealth _enemyBossHealth;
    CollisionKill _collisionKill;

    [Header("Values")]
    [SerializeField] List<GameObject> _triggeredHitObjectsVertical = new List<GameObject>(2);
    [SerializeField] List<GameObject> _triggeredHitObjectsHorizontal = new List<GameObject>(2);

    void Awake()
    {
        _enemyController = transform.parent.GetComponent<EnemyController>();
        _enemyBossHealth = transform.parent.GetComponent<EnemyBossHealth>();
        _collisionKill = GetComponent<CollisionKill>();
    }

    void OnEnable()
    {
        _collisionKill.OnKillDirection += HandleKillDirection;
        _collisionKill.OnClearTriggeredList += ClearTriggeredList;
    }

    void OnDisable()
    {
        _collisionKill.OnKillDirection -= HandleKillDirection;
        _collisionKill.OnClearTriggeredList -= ClearTriggeredList;
    }

    void ClearTriggeredList(Direction.Type direction)
    {
        if (direction == Direction.Type.Vertical) _triggeredHitObjectsVertical.Clear();
        if (direction == Direction.Type.Horizontal) _triggeredHitObjectsHorizontal.Clear();
    }

    void HandleKillDirection(Direction.Type direction, List<GameObject> hitObjects)
    {
        // Deal damage if the pair of hit objects aren't exactly the same
        switch (direction)
        {
            case Direction.Type.Vertical:
                if (hitObjects != _triggeredHitObjectsVertical)
                {
                    _enemyBossHealth.DealDamage();
                    _triggeredHitObjectsVertical = hitObjects;
                }
                break;

            case Direction.Type.Horizontal:
                if (hitObjects != _triggeredHitObjectsHorizontal)
                {
                    _enemyBossHealth.DealDamage();
                    _triggeredHitObjectsHorizontal = hitObjects;
                }
                break;
        }

        if (_enemyBossHealth.CurrentHealth == 0)
        {
            _enemyController.StateController.ChangeState(EnemyController.StateEnum.Death);

            // Scale the object according to the kill direction
            Vector3 toScale = Vector3.one;
            switch (direction)
            {
                case Direction.Type.Vertical:
                    toScale = new Vector3(_rootParent.localScale.x, 0, _rootParent.localScale.z);
                    break;

                case Direction.Type.Horizontal:
                    toScale = new Vector3(0, _rootParent.localScale.y, _rootParent.localScale.z);
                    break;
            }

            _rootParent.DOScale(toScale, _squeezeDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _rootParent.gameObject.SetActive(false);

                    GameState.ChangeState(GameState.Win);
                });
        }
    }
}
