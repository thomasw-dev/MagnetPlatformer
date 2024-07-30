using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject _player;

    [Range(1f, 10f)]
    [SerializeField] float _followSmoothFactor = 1f;
    [Range(1f, 10f)]
    [SerializeField] float _stickySmoothFactor = 1f;

    Camera _cam;
    bool _followPlayer = true;
    Vector3 _stickyPos;

    float _defaultZoom;
    float _currentZoom;
    Tweener _zoomTween;
    const float ZOOM_DURATION = 1f;

    void OnEnable()
    {
        CameraStickyArea.OnTriggerEnter += EnableSticky;
        CameraStickyArea.OnTriggerExit += DisableSticky;
    }

    void OnDisable()
    {
        CameraStickyArea.OnTriggerEnter -= EnableSticky;
        CameraStickyArea.OnTriggerExit -= DisableSticky;
    }

    void Awake()
    {
        _cam = Camera.main;
    }

    void Start()
    {
        _defaultZoom = _cam.orthographicSize;
    }

    void Update()
    {
        if (GameState.CurrentState != GameState.Lose)
        {
            if (_followPlayer)
            {
                Vector3 camPos = new Vector3(_player.transform.position.x, _player.transform.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, camPos, _followSmoothFactor * Time.deltaTime);
            }
            else
            {
                Vector3 stickyPos = new Vector3(_stickyPos.x, _stickyPos.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, stickyPos, _stickySmoothFactor * Time.deltaTime);
            }
        }
    }

    void EnableSticky(Vector3 pos, float zoom)
    {
        _followPlayer = false;
        _stickyPos = pos;
        if (zoom > 0)
        {
            _defaultZoom = _cam.orthographicSize;
            ZoomTransition(zoom);
        }
    }

    void DisableSticky()
    {
        _followPlayer = true;

        if (_cam.orthographicSize != _defaultZoom)
        {
            ZoomTransition(_defaultZoom);
        }
    }

    void ZoomTransition(float targetZoom)
    {
        // Kill any current zoom transition progress
        if (_zoomTween != null && _zoomTween.IsActive()) _zoomTween.Kill();

        // Start the zoom transition again
        _zoomTween = DOTween.To(x => _currentZoom = x, _cam.orthographicSize, targetZoom, ZOOM_DURATION)
            .SetEase(Ease.OutCubic)
            .OnUpdate(() =>
            {
                _cam.orthographicSize = _currentZoom;
            });

        _zoomTween.Play();
    }
}
