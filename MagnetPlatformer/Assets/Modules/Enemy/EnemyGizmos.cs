using UnityEngine;

public class EnemyGizmos : MonoBehaviour
{
    const float TARGET_POINT_SIZE = 0.2f;

    static Color CHASE_RADIUS_COLOR = Color.magenta;
    static Color TARGET_POINT_COLOR = Color.yellow;

    EnemyController GetController() => transform.parent.GetComponent<EnemyController>();

    EnemyValues GetValues() => transform.parent.GetComponent<EnemyValues>();

    void OnDrawGizmos()
    {
        if (DebugManager.EnemySettings.ChaseRadius)
        {
            Gizmos.color = CHASE_RADIUS_COLOR;
            Gizmos.DrawWireSphere(transform.position, GetValues().ChaseRadius);
        }

        if (DebugManager.EnemySettings.TargetPoint)
        {
            Gizmos.color = TARGET_POINT_COLOR;
            Gizmos.DrawWireSphere(GetController().TargetPoint, TARGET_POINT_SIZE);
        }
    }
}
