using DG.Tweening;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidbody2D;

    public enum StartMode { StartOnPlay, StartOnTrigger }
    [SerializeField] StartMode _startMode;
    [Tooltip("Only applicable to StartOnTrigger mode.")]
    [SerializeField] Collider2D _startTrigger;

    [SerializeField] bool _loop = true;

    [SerializeField] Marker[] _markers;

    [System.Serializable]
    public struct Marker
    {
        public Transform Point;
        public float Duration; // time it takes to move to this point
        public Ease Ease;
    }

    int _current = 0;

    void Start()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }

    void OnEnable()
    {
        if (_startMode == StartMode.StartOnPlay)
        {
            GameState.Play.OnEnter += StartPatrol;
        }
        GameState.Win.OnExit += StopPatrol;
        GameState.Lose.OnExit += StopPatrol;
    }

    void OnDisable()
    {
        if (_startMode == StartMode.StartOnPlay)
        {
            GameState.Play.OnEnter -= StartPatrol;
        }
        GameState.Win.OnExit -= StartPatrol;
        GameState.Lose.OnExit -= StopPatrol;
    }

    [ContextMenu("Start Patrol")]
    void StartPatrol()
    {
        ToNextMarker();
    }

    void ToNextMarker()
    {
        if (_current >= _markers.Length)
        {
            if (_loop)
            {
                _current = 0;
            }
            else return;
        }

        _rigidbody2D.DOMove(_markers[_current].Point.localPosition, _markers[_current].Duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            _current++;
            ToNextMarker();
        });
    }

    [ContextMenu("Stop Patrol")]
    void StopPatrol()
    {
        _rigidbody2D.DOKill();
        _rigidbody2D.position = _markers[0].Point.position;
        _current = 0;
    }
}
