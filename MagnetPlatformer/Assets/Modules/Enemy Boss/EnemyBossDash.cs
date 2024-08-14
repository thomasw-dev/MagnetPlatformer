using DG.Tweening;
using UnityEngine;

public class EnemyBossDash : MonoBehaviour
{
    EnemyBossController _enemyBossController;
    EnemyBossValues _enemyBossValues;
    Transform _player;

    [SerializeField] float _dashEnableDistance = 5f;
    bool dashCountdownStarted = false;

    Tweener _dashCountdownTween;
    float _dashCountdownProgress;

    Tweener _dashAccelerationTween;
    float _dashAccelerationProgress;

    [SerializeField] bool _dash = false;
    [SerializeField] float _dashAcceleration = 500f;
    [SerializeField] float _initialAcceleration = 500f;
    [SerializeField] float _dashDuration = 1f;
    float _initialChaseAcceleration;

    public bool ReadyToDash;
    public float TimeToDash;
    bool _dashed = false;

    void Awake()
    {
        _enemyBossController = GetComponent<EnemyBossController>();
        _enemyBossValues = GetComponent<EnemyBossValues>();
        _player = Method.GetPlayerObject().transform;
    }

    void Start()
    {
        _initialChaseAcceleration = _enemyBossValues.ChaseAcceleration;
    }

    void Update()
    {
        _enemyBossValues.ChaseAcceleration = _dash ? _dashAcceleration : _initialAcceleration;

        return;

        ReadyToDash = PlayerIsInSameDirection() && Mathf.Abs(transform.position.x - _player.position.x) >= _dashEnableDistance;

        if (ReadyToDash)
        {
            if (!dashCountdownStarted)
            {
                StartDashCountdown();
                dashCountdownStarted = true;
            }

            if (Time.time >= TimeToDash)
            {
                Dash();
                _dashed = true;
            }
        }
        else
        {
            _dashed = false;
        }
    }

    bool PlayerIsInSameDirection()
    {
        bool left = _enemyBossController.MoveDirection == Move.Direction.Left && _player.position.x < transform.position.x;
        bool right = _enemyBossController.MoveDirection == Move.Direction.Right && _player.position.x > transform.position.x;
        return left || right;
    }

    void StartDashCountdown()
    {
        float timeUntilNextDash = GetNextDashCountdown();
        TimeToDash = Time.time + timeUntilNextDash;
    }

    float GetNextDashCountdown()
    {
        float min = 2f;
        float max = 4f;
        return Random.Range(min, max);
    }

    void Dash()
    {
        // Kill any current tween progress
        if (_dashAccelerationTween != null && _dashAccelerationTween.IsActive()) _dashAccelerationTween.Kill();

        // Start the tween again
        _dashAccelerationTween = DOTween.To(x => _dashAccelerationProgress = x, _dashAcceleration, _initialChaseAcceleration, _dashDuration)
            .SetEase(Ease.Linear)
            .SetAutoKill(false)
            .OnPlay(() =>
            {
                Debug.Log("Dash tween start.");
            })
            .OnUpdate(() =>
            {
                _enemyBossValues.ChaseAcceleration = _dashAccelerationProgress;
            })
            .OnComplete(() =>
            {
                Debug.Log("Dash tween complete.");
            });

        _dashAccelerationTween.Play();
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
