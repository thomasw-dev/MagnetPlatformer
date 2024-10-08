using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float _startZoom = 4f;
    [SerializeField] float _defaultZoom = 9f;
    [SerializeField] bool _followPlayer = true;
    [Range(1f, 10f)]
    [SerializeField] float _followSmoothFactor = 4f;
    [SerializeField] Vector3 _followOffset = Vector2.zero;

    Camera _cam;
    Transform _player;

    float _currentZoom;
    Tweener _zoomTween;
    const float ZOOM_DURATION = 1f;

    void Awake()
    {
        _cam = GetComponent<Camera>();
        _player = Method.GetPlayerObject().transform;
    }

    void Start()
    {
        _cam.orthographicSize = _startZoom;
        EnableDefaultBehaviour();
    }

    void Update()
    {
        if (GameState.CurrentState != GameState.Play) { return; }

        if (_followPlayer && _player != null)
        {
            Vector3 newPos = new Vector3(_player.transform.position.x, _player.transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPos + _followOffset, _followSmoothFactor * Time.deltaTime);
        }
    }

    public void EnableDefaultBehaviour()
    {
        _followPlayer = true;
        if (_cam.orthographicSize != _defaultZoom)
        {
            ZoomTransition(_defaultZoom);
        }
    }

    public void DisableDefaultBehaviour()
    {
        _followPlayer = false;
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

    [ContextMenu("Move To Player")]
    void MoveToPlayer()
    {
        Vector3 to = Method.GetPlayerObject().transform.position;
        transform.position = new Vector3(to.x, to.y, -10);
    }
}
