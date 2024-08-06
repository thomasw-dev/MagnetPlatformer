using System.Collections;
using UnityEngine;

public class EnemyCollisionKill : MonoBehaviour
{
    [Header("Dependencies")] // Required to be assigned in the Inspector
    [SerializeField] Transform _rootParent;

    const float KILL_SET_INACTIVE_WAIT = 0.1f;

    CollisionKill _collisionKill;

    void Awake()
    {
        _collisionKill = GetComponent<CollisionKill>();
    }

    void OnEnable()
    {
        _collisionKill.OnKill += HandleKill;
    }

    void OnDisable()
    {
        _collisionKill.OnKill -= HandleKill;
    }

    void HandleKill()
    {
        StartCoroutine(WaitSetInactive());
    }

    IEnumerator WaitSetInactive()
    {
        yield return new WaitForSeconds(KILL_SET_INACTIVE_WAIT);
        _rootParent.gameObject.SetActive(false);
    }
}
