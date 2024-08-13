using DG.Tweening;
using UnityEngine;

public class EnemyBossDash : MonoBehaviour
{
    EnemyController _enemyController;
    EnemyValues _enemyValues;
    Transform _player;

    [SerializeField] bool _playerIsInSameDirection;

    [SerializeField] float _dashEnableDistance = 5f;

    Tweener _dashCountdownTween;
    float _dashCountdownProgress;

    void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
        _enemyValues = GetComponent<EnemyValues>();
        _player = Method.GetPlayerObject().transform;
    }

    void Update()
    {
        /*if (PlayerIsInSameDirection())
        {

        }*/

        _playerIsInSameDirection = PlayerIsInSameDirection();
    }

    bool PlayerIsInSameDirection()
    {
        bool left = _enemyController.MoveDirection == Move.Direction.Left && _player.position.x < transform.position.x;
        bool right = _enemyController.MoveDirection == Move.Direction.Right && _player.position.x > transform.position.x;
        return left || right;
    }

    void StartDashCountdown()
    {
        float nextDashCountdown = GetNextDashCountdown();
    }

    float GetNextDashCountdown()
    {
        float min = 2f;
        float max = 4f;
        return Random.Range(min, max);
    }

    void OnDrawGizmos()
    {
        if (GizmosSettings.Enemy.DashEnableDistance)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.left * _dashEnableDistance);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * _dashEnableDistance);
        }
    }
}
