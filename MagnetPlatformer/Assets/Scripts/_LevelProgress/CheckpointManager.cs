using System;
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

    void OnEnable()
    {
        foreach (Checkpoint checkpoint in _checkpoints)
        {
            checkpoint.OnPlayerReach += SaveProgress;
        }
    }

    void OnDisable()
    {
        foreach (Checkpoint checkpoint in _checkpoints)
        {
            checkpoint.OnPlayerReach -= SaveProgress;
        }
    }

    void Start()
    {
        if (_startAtCurrentPosition) { return; }
        if (_levelProgress == null) { return; }

        GoToCheckpoint(_levelProgress.CurrentCheckpoint);
    }

    void GoToCheckpoint(int i)
    {
        _camera.position = new Vector3(_checkpoints[i].transform.position.x, _checkpoints[i].transform.position.y, _camera.position.z);
        _player.position = new Vector3(_checkpoints[i].transform.position.x, _checkpoints[i].transform.position.y, _player.position.z);
    }

    void SaveProgress(Checkpoint checkpoint)
    {
        _levelProgress.SaveCheckpointIndex(Array.IndexOf(_checkpoints, checkpoint));
    }
}
