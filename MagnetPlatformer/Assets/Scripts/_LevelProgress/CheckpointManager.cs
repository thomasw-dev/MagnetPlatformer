using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] LevelProgress _levelProgress;
    [SerializeField] Checkpoint[] _checkpoints;

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
            GoToCheckpoint(0);
        }
    }

    void GoToCheckpoint(int i)
    {
        _camera.position = _checkpoints[0].transform.position;
        _player.position = _checkpoints[0].transform.position;
    }
}
