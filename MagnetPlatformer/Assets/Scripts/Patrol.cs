using DG.Tweening;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public enum StartMode { StartOnPlay, StartOnTrigger }
    [SerializeField] StartMode _startMode;
    [Tooltip("Only applicable for StartOnTrigger start mode.")]
    [SerializeField] Collider2D _startTrigger;

    [SerializeField] Marker[] _markers;

    [System.Serializable]
    public struct Marker
    {
        public Transform point;
        public float duration;
    }

    Rigidbody2D _rigidbody2D;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
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
        for (int i = 0; i < _markers.Length; i++)
        {
            //bool complete = false;
            _rigidbody2D.DOMove(_markers[i].point.position, _markers[i].duration);
        }
    }

    [ContextMenu("Stop Patrol")]
    void StopPatrol()
    {
        _rigidbody2D.DOKill();
    }
}
