using DG.Tweening;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidbody2D;

    public enum StartMode { StartOnPlay, StartOnTrigger }
    [SerializeField] StartMode _startMode;

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

    void OnTriggerEnter2D(Collider2D col)
    {
        if (Method.IsPlayerObject(col.gameObject))
        {
            StartPatrol();
        }
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

        Vector2 position = transform.TransformPoint(_markers[_current].Point.localPosition);
        float duration = _markers[_current].Duration;
        Ease ease = _markers[_current].Ease;
        _rigidbody2D.DOMove(position, duration).SetEase(ease).OnComplete(() =>
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
