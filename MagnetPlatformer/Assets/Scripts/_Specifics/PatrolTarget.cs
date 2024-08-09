using UnityEngine;

public class PatrolTarget : MonoBehaviour
{
    [SerializeField] GameObject _target;

    [ContextMenu("Update To Target (Move & Rename)")]
    void UpdateToTarget()
    {
        transform.position = _target.transform.position;
        gameObject.name = _target.name + " Patrol Points";
    }
}
