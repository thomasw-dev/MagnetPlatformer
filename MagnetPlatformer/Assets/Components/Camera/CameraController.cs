using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject _player;

    [Range(1f, 10f)]
    [SerializeField] float _followSmoothFactor = 1f;
    [Range(1f, 10f)]
    [SerializeField] float _stickySmoothFactor = 1f;

    bool _followPlayer = true;
    Vector3 _stickyPos;

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

    void Update()
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

    void EnableSticky(Vector3 pos)
    {
        _followPlayer = false;
        _stickyPos = pos;
    }

    void DisableSticky()
    {
        _followPlayer = true;
    }
}
