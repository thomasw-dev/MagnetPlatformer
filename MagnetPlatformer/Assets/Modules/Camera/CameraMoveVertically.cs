using UnityEngine;

public class CameraMoveVertically : CameraTriggerArea
{
    [SerializeField] Transform _widthPoint;
    [Range(1f, 10f)]
    [SerializeField] float _lerpFactor = 4f;

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
        if (_widthPoint == null) { return; }

        Vector3 heightPos = new Vector3(_widthPoint.position.x, _player.transform.position.y, _camera.transform.position.z);
        Vector3 newPos = Vector3.Lerp(_camera.transform.position, heightPos, _lerpFactor * Time.deltaTime);

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
