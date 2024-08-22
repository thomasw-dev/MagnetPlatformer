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
        _camera.position = new Vector3(_checkpoints[0].transform.position.x, _checkpoints[0].transform.position.y, _camera.position.z);
        _player.position = new Vector3(_checkpoints[0].transform.position.x, _checkpoints[0].transform.position.y, _player.position.z);
    }
}
