public class CameraDisableDefaultBehaviour : ICameraTriggerArea
{
    void Update()
    {
        if (!_effectEnabled) { return; }
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
