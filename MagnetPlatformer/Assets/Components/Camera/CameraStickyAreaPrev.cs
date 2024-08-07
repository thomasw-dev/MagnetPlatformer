using System;
using UnityEngine;

public class CameraStickyAreaPrev : MonoBehaviour
{
    public static event Action<Vector3, float> OnTriggerEnter;
    public static event Action OnTriggerExit;

    [SerializeField] float _zoom = 0f;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (Method.IsPlayerObject(col.gameObject))
        {
            OnTriggerEnter?.Invoke(transform.position, _zoom);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (Method.IsPlayerObject(col.gameObject))
        {
            OnTriggerExit?.Invoke();
        }
    }

    [ContextMenu("Move Camera To Here")]
    void MoveCameraToSelf()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        if (_zoom > 0)
        {
            Camera.main.orthographicSize = _zoom;
        }
    }
}
