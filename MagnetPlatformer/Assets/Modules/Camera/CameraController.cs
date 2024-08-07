using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _startPoint;
    [SerializeField] float _defaultZoom = 9f;

    [SerializeField] bool _followPlayer = true;

    [Range(1f, 10f)]
    [SerializeField] float _followSmoothFactor = 4f;
    [SerializeField] Vector3 _followOffset = Vector2.zero;

    Camera _cam;
    Transform _player;

    void Awake()
    {
        _cam = GetComponent<Camera>();
        _player = Method.GetPlayerObject().transform;
    }

    void Start()
    {
        transform.position = _startPoint.transform.position;
        EnableDefaultBehaviour();
    }

    void Update()
    {
        if (_followPlayer && _player != null)
        {
            Vector3 playerPos = new Vector3(_player.transform.position.x, _player.transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, playerPos + _followOffset, _followSmoothFactor * Time.deltaTime);
        }
    }

    public void EnableDefaultBehaviour()
    {
        _followPlayer = true;
        _cam.orthographicSize = _defaultZoom;
    }

    public void DisableDefaultBehaviour()
    {
        _followPlayer = false;
    }

    [ContextMenu("Save Current Position To Start Point")]
    void SaveCurrentPosToStartPoint()
    {
        if (_startPoint == null) { return; }
        _startPoint.transform.position = transform.position;
    }
}
