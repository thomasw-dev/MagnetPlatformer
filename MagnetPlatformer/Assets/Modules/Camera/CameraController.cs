using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Vector3 _startingPos = new Vector3(0, 0, -10f);

    [ContextMenu("Set This Position As Starting Position")]
    void SetStartingPosition()
    {
        _startingPos = transform.position;
    }
}
