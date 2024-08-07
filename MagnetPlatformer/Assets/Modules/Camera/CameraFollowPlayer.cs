using UnityEngine;

public class CameraFollowPlayer : ICameraTriggerArea
{
    [SerializeField] Vector3 _offset;

    Camera _camera;
    Transform _target;

    void Awake()
    {
        _camera = Camera.main;
        _target = Method.GetPlayerObject().transform;
    }

    void Update()
    {
        _camera.transform.position = _target.position;
    }

    // follow pos keeps lerping to player position

    public override void OnTriggerEnter2D()
    {
        // Transition cam pos to the follow pos
    }

    public override void OnTriggerExit2D()
    {

    }
}
