using UnityEngine;

public class MoveObjectToSelf : MonoBehaviour
{
    [SerializeField] Transform _object;

    [ContextMenu("Move Object To Self")]
    void MoveToSelf()
    {
        _object.transform.position = transform.position;
    }
}
