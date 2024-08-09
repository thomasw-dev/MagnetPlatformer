using UnityEngine;

public class EnemyBossMoveTarget : MonoBehaviour
{
    Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _animator.enabled = false;
    }

    [ContextMenu("Start Patrol")]
    public void StartPatrol()
    {
        _animator.enabled = true;
    }

    [ContextMenu("Stop Patrol")]
    public void StopPatrol()
    {
        _animator.enabled = false;
    }
}
