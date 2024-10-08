using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyCollisionKill : MonoBehaviour
{
    [Header("Dependencies")] // Required to be assigned in the Inspector
    [SerializeField] Transform _rootParent;

    [SerializeField] float _squeezeDuration = 1f;

    EnemyController _enemyController;
    CollisionKill _collisionKill;

    void Awake()
    {
        _enemyController = transform.parent.GetComponent<EnemyController>();
        _collisionKill = GetComponent<CollisionKill>();
    }

    void OnEnable()
    {
        _collisionKill.OnKillDirection += HandleKillDirection;
    }

    void OnDisable()
    {
        _collisionKill.OnKillDirection -= HandleKillDirection;
    }

    void HandleKillDirection(Direction.Type direction, List<GameObject> hitObjects)
    {
        _enemyController.StateController.ChangeState(EnemyController.StateEnum.Death);

        Vector3 toScale = Vector3.one;
        if (direction == Direction.Type.Vertical) toScale = new Vector3(_rootParent.localScale.x, 0, _rootParent.localScale.z);
        if (direction == Direction.Type.Horizontal) toScale = new Vector3(0, _rootParent.localScale.y, _rootParent.localScale.z);

        _rootParent.DOScale(toScale, _squeezeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _rootParent.gameObject.SetActive(false);
            });
    }
}
