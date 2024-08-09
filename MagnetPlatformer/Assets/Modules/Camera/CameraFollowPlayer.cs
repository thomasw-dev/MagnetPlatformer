using UnityEngine;

public class CameraFollowPlayer : CameraTriggerArea
{
    [Range(1f, 10f)]
    [SerializeField] float _lerpFactor = 4f;
    [SerializeField] Vector3 _followOffset = Vector2.zero;

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

        Vector3 newPos = new Vector3(_player.transform.position.x, _player.transform.position.y, _camera.transform.position.z);
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, newPos + _followOffset, _lerpFactor * Time.deltaTime);
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
