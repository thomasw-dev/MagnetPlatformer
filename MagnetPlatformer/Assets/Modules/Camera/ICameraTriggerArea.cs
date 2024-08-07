using UnityEngine;

public class ICameraTriggerArea : MonoBehaviour
{
    protected Camera _camera;
    protected CameraController _cameraController;
    protected bool _effectEnabled = false;

    protected virtual void Awake()
    {
        _camera = Camera.main;
        _cameraController = _camera.GetComponent<CameraController>();
    }

    void OnDisable() => DisableEffect();

    void OnTriggerEnter2D(Collider2D collider) => EnableEffect();

    void OnTriggerStay2D(Collider2D collider) => EnableEffect();

    void OnTriggerExit2D(Collider2D collider) => DisableEffect();

    protected virtual void EnableEffect()
    {
        if (_effectEnabled) { return; }
        _effectEnabled = true;
    }

    protected virtual void DisableEffect()
    {
        _effectEnabled = false;
    }
}
