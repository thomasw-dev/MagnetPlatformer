using UnityEngine;

[ExecuteInEditMode]
public class EditorToggleCameraFollowTarget : MonoBehaviour
{
    [SerializeField] Transform _camera;
    [SerializeField] Transform _target;
    [SerializeField] bool _following;
    [SerializeField] Vector2 _offset;

    void Start()
    {
        if (Application.isPlaying)
        {
            // Disable the script when entering Play mode
            enabled = false;
        }
    }

    void Update()
    {
        if (_following)
        {
            Vector3 offset = new Vector3(_offset.x, _offset.y, 0);
            _camera.position = new Vector3(_target.position.x, _target.position.y, _camera.position.z) + offset;
        }
    }
}
