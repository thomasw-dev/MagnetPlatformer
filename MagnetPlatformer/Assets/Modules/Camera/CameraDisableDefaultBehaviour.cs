public class CameraDisableDefaultBehaviour : CameraTriggerArea
{
    void Update()
    {
        if (!_effectEnabled) { return; }
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
