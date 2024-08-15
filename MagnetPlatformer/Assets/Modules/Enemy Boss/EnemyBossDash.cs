using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBossDash : MonoBehaviour
{
    [Header("Countdown Condition")]

    [SerializeField] bool _playerIsInSameDirection;
    [SerializeField] bool _playerIsFartherThanMinDistance;
    [SerializeField] bool _dashConditionIsMet;
    bool _dashCondition
    {
        set
        {
            if (value == true && !CountingDown) OnDashConditionMet?.Invoke();
            if (value == false) OnDashConditionUnmet?.Invoke();
            _dashConditionIsMet = value;
        }
    }
    event Action OnDashConditionMet;
    event Action OnDashConditionUnmet;

    EnemyBossController _enemyBossController;
    Transform _player;

    [Header("Countdown")]

    [SerializeField] float _minDistanceToEnable = 5f;
    public bool CountingDown;
    Tweener _countdownToNextDashTween;
    float _countdownToNextDashProgress;
    public float NextDashTime;

    [Header("Dash")]

    public bool Dashing = false;
    Tweener _dashAccelerationTween;
    float _dashAccelerationProgress;
    [SerializeField] float _dashAcceleration = 500f;
    [SerializeField] float _dashDuration = 1f;
    float _initialChaseAcceleration;

    public event Action OnDashStart;
    public event Action OnDashStop;

    [Header("Debug")]

    [SerializeField] bool _manualDashTrigger = false;

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
        _dashCondition = !Dashing && PlayerIsInSameDirection() && PlayerIsFartherThanMinDistance();

        if (_manualDashTrigger) Dash();

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
        float min = 2f;
        float max = 4f;
        return Random.Range(min, max);
    }

    void StartCountdownToNextDash()
    {
        float duration = GetCountdownDurationToNextDash();
        NextDashTime = Time.time + duration;

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
                    Debug.Log($"Next dash in {durationDisplay} seconds.");
                }
            })
            .OnUpdate(() =>
            {
                CountingDown = true;
            })
            .OnComplete(() =>
            {
                Dash();
                CountingDown = false;
            })
            .OnKill(() =>
            {
                CountingDown = false;
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

                if (_manualDashTrigger) _manualDashTrigger = false;
                if (Log.EnemyBoss) Debug.Log($"Dash started.");
            })
            .OnUpdate(() =>
            {
                Dashing = true;
                _enemyBossController.Values.ChaseAcceleration = _dashAccelerationProgress;
            })
            .OnComplete(() =>
            {
                Dashing = false;
                OnDashStop?.Invoke();
                _enemyBossController.Values.ChaseAcceleration = _initialChaseAcceleration;

                if (Log.EnemyBoss) Debug.Log($"Dash stopped.");
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
