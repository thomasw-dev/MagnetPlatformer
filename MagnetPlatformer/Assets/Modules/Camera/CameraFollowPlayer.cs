using UnityEngine;

public class CameraFollowPlayer : ICameraTriggerArea
{
    [SerializeField] Vector3 _offset;

    Transform _target;

    protected override void Awake()
    {
        base.Awake();
        _target = Method.GetPlayerObject().transform;
    }

    void Update()
    {
        if (_effectEnabled) { return; }
    }

    protected override void EnableEffect()
    {
        if (_cameraController != null)
        {
            _cameraController.DisableDefaultBehaviour();
        }
    }

    protected override void DisableEffect()
    {
        if (_cameraController != null)
        {
            _cameraController.EnableDefaultBehaviour();
        }
    }
}
