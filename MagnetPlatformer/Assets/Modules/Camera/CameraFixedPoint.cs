using DG.Tweening;
using UnityEngine;

public class CameraFixedPointAdjustZoom : CameraTriggerArea
{
    [SerializeField] Transform _point;

    [Range(0f, 10f)]
    [SerializeField] float _lerpFactor = 4f;

    [Range(0f, 30f)]
    [SerializeField] float _zoom = 9f;

    Tweener _zoomTween;
    float _currentZoom;
    [SerializeField] Ease _ease = Ease.OutCubic;
    float _zoomDuration = 1f;

    [SerializeField] Vector3 _offset;

    void Update()
    {
        if (!_effectEnabled) { return; }
        if (_point == null) { return; }

        Vector3 newPos = new Vector3(_point.position.x, _point.position.y, _camera.transform.position.z) + _offset;
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, newPos, _lerpFactor * Time.deltaTime);

        if (_camera.orthographicSize != _zoom)
        {
            ZoomTransition(_zoom);
        }
    }

    void ZoomTransition(float targetZoom)
    {
        // Kill any current zoom transition progress
        if (_zoomTween != null && _zoomTween.IsActive()) _zoomTween.Kill();

        // Start the zoom transition again
        _zoomTween = DOTween.To(x => _currentZoom = x, _camera.orthographicSize, targetZoom, _zoomDuration)
            .SetEase(_ease)
            .OnUpdate(() =>
            {
                _camera.orthographicSize = _currentZoom;
            });

        _zoomTween.Play();
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
