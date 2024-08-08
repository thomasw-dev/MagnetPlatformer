using UnityEngine;

public class CameraMoveHorizontally : CameraTriggerArea
{
    [SerializeField] Transform _heightPoint;
    [Range(1f, 10f)]
    [SerializeField] float _followSmoothFactor = 4f;

    Transform _player;

    protected override void Awake()
    {
        base.Awake();
        _player = Method.GetPlayerObject().transform;
    }

    void Update()
    {
        if (!_effectEnabled) { return; }

        if (_player == null) { return; }
        if (_heightPoint == null) { return; }

        Vector3 heightPos = new Vector3(_player.transform.position.x, _heightPoint.position.y, _camera.transform.position.z);
        Vector3 newPos = Vector3.Lerp(_camera.transform.position, heightPos, _followSmoothFactor * Time.deltaTime);

        _camera.transform.position = newPos;
    }

    protected override void EnableEffect()
    {
        base.EnableEffect();
        if (_cameraController != null)
        {
            _cameraController.DisableDefaultBehaviour();
        }
    }

    protected override void DisableEffect()
    {
        base.DisableEffect();
        if (_cameraController != null)
        {
            _cameraController.EnableDefaultBehaviour();
        }
    }
}
