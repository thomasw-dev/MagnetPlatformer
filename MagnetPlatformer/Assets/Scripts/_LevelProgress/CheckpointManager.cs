using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] LevelProgress _levelProgress;
    [SerializeField] Checkpoint[] _checkpoints;

    [SerializeField] Transform _cameraStartPoint;
    [SerializeField] Transform _playerStartPoint;

    [Header("Options")]
    [SerializeField] bool _startAtCurrentPosition = false;

    Transform _camera;
    Transform _player;

    void Awake()
    {
        _camera = Camera.main.transform;
        _player = Method.GetPlayerObject().transform;
    }

    void Start()
    {
        if (!_startAtCurrentPosition)
        {
            _camera.position = _cameraStartPoint.position;
            _player.position = _playerStartPoint.position;
        }
    }
}
