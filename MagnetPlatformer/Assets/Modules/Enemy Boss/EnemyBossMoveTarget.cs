using DG.Tweening;
using UnityEngine;

public class EnemyBossMoveTarget : MonoBehaviour
{
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

    void OnEnable()
    {
        GameState.Play.OnExit += StopPatrol;
    }

    void OnDisable()
    {
        GameState.Play.OnExit -= StopPatrol;
    }

    [ContextMenu("Start Patrol")]
    public void StartPatrol()
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

        Vector2 position = _markers[0].Point.TransformPoint(_markers[_current].Point.localPosition);
        float duration = _markers[_current].Duration;
        Ease ease = _markers[_current].Ease;
        transform.DOMove(position, duration).SetEase(ease).OnComplete(() =>
        {
            _current++;
            ToNextMarker();
        });
    }

    [ContextMenu("Stop Patrol")]
    public void StopPatrol()
    {
        transform.DOKill();
        transform.position = _markers[0].Point.position;
        _current = 0;
    }
}
