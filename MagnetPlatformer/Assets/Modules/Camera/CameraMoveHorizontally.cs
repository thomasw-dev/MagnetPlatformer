using DG.Tweening;
using UnityEngine;

public class CameraMoveHorizontally : CameraTriggerArea
{
    [SerializeField] Transform _heightPoint;
    [Range(1f, 10f)]
    [SerializeField] float _lerpFactor = 4f;
    [Range(0f, 30f)]
    [SerializeField] float _zoom = 9f;
    [SerializeField] Vector3 _offset;

    float _currentZoom;
    Tweener _zoomTween;
    const float ZOOM_DURATION = 1f;

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

        Vector3 heightPos = new Vector3(_player.transform.position.x, _heightPoint.position.y, _camera.transform.position.z) + _offset;
        Vector3 newPos = Vector3.Lerp(_camera.transform.position, heightPos, _lerpFactor * Time.deltaTime);

        _camera.transform.position = newPos;

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
        _zoomTween = DOTween.To(x => _currentZoom = x, _camera.orthographicSize, targetZoom, ZOOM_DURATION)
            .SetEase(Ease.OutCubic)
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
