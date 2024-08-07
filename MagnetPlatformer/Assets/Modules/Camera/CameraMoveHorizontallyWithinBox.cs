using UnityEngine;

public class CameraMoveHorizontallyWithinBox : ICameraTriggerArea
{
    [SerializeField] BoxCollider2D _constraintArea;
    [SerializeField] Transform _heightPoint;

    [Range(1f, 10f)]
    [SerializeField] float _followSmoothFactor = 1f;

    Transform _player;

    protected override void Awake()
    {
        base.Awake();
        _player = Method.GetPlayerObject().transform;
    }

    void Update()
    {
        if (!_effectEnabled) { return; }

        Vector3 newPos = Vector3.zero;

        if (_heightPoint != null && _player != null)
        {
            Vector3 heightPos = new Vector3(_player.transform.position.x, _heightPoint.position.y, _camera.transform.position.z);
            newPos = Vector3.Lerp(_camera.transform.position, heightPos, _followSmoothFactor * Time.deltaTime);
        }

        Vector3 constrainedPos = newPos;
        //constrainedPos.x = Mathf.Clamp(constrainedPos.x, newPos.x + _constraintArea.offset.x - (_constraintArea.size.x / 2f), newPos.x + _constraintArea.offset.x + (_constraintArea.size.x / 2f));
        //constrainedPos.y = Mathf.Clamp(constrainedPos.y, newPos.y + _constraintArea.offset.y - (_constraintArea.size.y / 2f), newPos.y + _constraintArea.offset.y + (_constraintArea.size.y / 2f));

        _camera.transform.position = constrainedPos;
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
