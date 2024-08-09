using UnityEngine;

public class CameraFixedPoint : CameraTriggerArea
{
    [SerializeField] Transform _point;
    [Range(1f, 10f)]
    [SerializeField] float _lerpFactor = 4f;

    void Update()
    {
        if (!_effectEnabled) { return; }

        if (_point != null)
        {
            Vector3 newPos = new Vector3(_point.position.x, _point.position.y, _camera.transform.position.z);
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, newPos, _lerpFactor * Time.deltaTime);
        }
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
