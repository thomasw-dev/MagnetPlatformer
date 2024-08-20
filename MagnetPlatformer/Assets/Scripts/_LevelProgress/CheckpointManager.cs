using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] LevelProgress _levelProgress;
    [SerializeField] Checkpoint[] _checkpoints;

    [SerializeField] Transform _manualStartPoint;

    [Header("Options")]
    [SerializeField] bool _startAtCurrentPosition = false;

    CheckpointManager _checkpointManager;
    Transform _player;

    void Awake()
    {
        _checkpointManager = GetComponent<CheckpointManager>();
        _player = Method.GetPlayerObject().transform;
    }

    void Start()
    {
        if (_manualStartPoint != null && !_startAtCurrentPosition)
        {
            transform.position = _manualStartPoint.position;
        }
    }
}
