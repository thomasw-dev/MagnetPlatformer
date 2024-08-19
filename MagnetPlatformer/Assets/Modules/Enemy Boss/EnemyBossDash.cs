using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBossDash : MonoBehaviour
{
    [Header("Countdown Settings")]

    [SerializeField] float _minDistanceToEnable = 5f;
    [SerializeField] float _randomCountdownMin = 2f;
    [SerializeField] float _randomCountdownMax = 4f;
    [SerializeField] bool _useManualCountdown = false;
    [Range(0f, 10f)]
    [SerializeField] float _manualCountdown = 1f;

    [Header("Countdown Condition")]

    [SerializeField] bool _playerIsInSameDirection;
    [SerializeField] bool _playerIsFartherThanMinDistance;
    [SerializeField] bool _dashConditionIsMet;
    bool _dashCondition
    {
        set
        {
            if (value == true && !IsCountingDown) OnDashConditionMet?.Invoke();
            if (value == false) OnDashConditionUnmet?.Invoke();
            _dashConditionIsMet = value;
        }
    }
    event Action OnDashConditionMet;
    event Action OnDashConditionUnmet;

    [Header("Countdown")]

    public bool IsCountingDown = false;
    Tweener _countdownToNextDashTween;
    float _countdownToNextDashProgress;
    float _nextDashTime;
    public float NextDashIn;

    [Header("Dash")]

    public bool IsDashing = false;
    Tweener _dashAccelerationTween;
    float _dashAccelerationProgress;
    [SerializeField] float _dashAcceleration = 500f;
    [SerializeField] float _dashDuration = 1f;
    float _initialChaseAcceleration;

    public event Action OnDashStart;
    public event Action OnDashStop;

    EnemyBossController _enemyBossController;
    Transform _player;

    void Awake()
    {
        _enemyBossController = GetComponent<EnemyBossController>();
        _player = Method.GetPlayerObject().transform;
    }

    void OnEnable()
    {
        _enemyBossController.StateController.EnumToState(EnemyBossController.StateEnum.Chase).OnEnter += StartCountdownToNextDash;
        OnDashConditionMet += StartCountdownToNextDash;
        OnDashConditionUnmet += KillCountdownToNextDash;
    }

    void OnDisable()
    {
        _enemyBossController.StateController.EnumToState(EnemyBossController.StateEnum.Chase).OnEnter -= StartCountdownToNextDash;
        OnDashConditionMet -= StartCountdownToNextDash;
        OnDashConditionUnmet -= KillCountdownToNextDash;
    }

    void Update()
    {
        _dashCondition = !IsDashing && PlayerIsInSameDirection() && PlayerIsFartherThanMinDistance();

        NextDashIn = _nextDashTime - Time.time;

        _playerIsInSameDirection = PlayerIsInSameDirection();
        _playerIsFartherThanMinDistance = PlayerIsFartherThanMinDistance();
    }

    bool PlayerIsInSameDirection()
    {
        bool left = _enemyBossController.MoveDirection == Move.Direction.Left && _player.position.x < transform.position.x;
        bool right = _enemyBossController.MoveDirection == Move.Direction.Right && _player.position.x > transform.position.x;
        return left || right;
    }

    bool PlayerIsFartherThanMinDistance()
    {
        return Mathf.Abs(transform.position.x - _player.position.x) >= _minDistanceToEnable;
    }

    float GetCountdownDurationToNextDash()
    {
        if (_useManualCountdown) return _manualCountdown;
        else return Random.Range(_randomCountdownMin, _randomCountdownMax);
    }

    void StartCountdownToNextDash()
    {
        float duration = GetCountdownDurationToNextDash();
        _nextDashTime = Time.time + duration;

        // Kill any current tween progress
        KillCountdownToNextDash();

        // Start the tween again
        _countdownToNextDashTween = DOTween.To(x => _countdownToNextDashProgress = x, duration, 0, duration)
            .SetEase(Ease.Linear)
            .SetAutoKill(false)
            .OnPlay(() =>
            {
                if (Log.EnemyBoss)
                {
                    string durationDisplay = duration.ToString("F2");
                    Debug.Log($"Enemy Boss: Next dash in {durationDisplay} seconds.");
                }
            })
            .OnUpdate(() =>
            {
                IsCountingDown = true;
            })
            .OnComplete(() =>
            {
                Dash();
                IsCountingDown = false;
            })
            .OnKill(() =>
            {
                IsCountingDown = false;
            });

        _countdownToNextDashTween.Play();
    }

    void KillCountdownToNextDash()
    {
        if (_countdownToNextDashTween != null && _countdownToNextDashTween.IsActive())
            _countdownToNextDashTween.Kill();
    }

    void Dash()
    {
        // Kill any current tween progress
        if (_dashAccelerationTween != null && _dashAccelerationTween.IsActive()) _dashAccelerationTween.Kill();

        // Start the tween again
        _dashAccelerationTween = DOTween.To(x => _dashAccelerationProgress = x, _dashAcceleration, _dashAcceleration, _dashDuration)
            .SetEase(Ease.Linear)
            .SetAutoKill(false)
            .OnPlay(() =>
            {
                OnDashStart?.Invoke();
                _initialChaseAcceleration = _enemyBossController.Values.ChaseAcceleration;

                if (Log.EnemyBoss) Debug.Log($"Enemy boss begins dash.");
            })
            .OnUpdate(() =>
            {
                IsDashing = true;
                _enemyBossController.Values.ChaseAcceleration = _dashAccelerationProgress;
            })
            .OnComplete(() =>
            {
                IsDashing = false;
                OnDashStop?.Invoke();
                _enemyBossController.Values.ChaseAcceleration = _initialChaseAcceleration;

                if (Log.EnemyBoss) Debug.Log($"Enemy boss ends dash.");
            });

        _dashAccelerationTween.Play();
    }

    void OnDrawGizmos()
    {
        if (GizmosSettings.Enemy.DashEnableDistance)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _minDistanceToEnable);
        }
    }
}
