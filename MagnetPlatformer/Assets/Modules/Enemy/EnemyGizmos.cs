using UnityEngine;

public class EnemyGizmos : MonoBehaviour
{
    static Color CHASE_RADIUS_COLOR = Color.magenta;

    EnemyValues GetValues() => transform.parent.GetComponent<EnemyValues>();

    void OnDrawGizmos()
    {
        if (DebugManager.EnemySettings.ChaseRadius)
        {
            Gizmos.color = CHASE_RADIUS_COLOR;
            Gizmos.DrawWireSphere(transform.position, GetValues().ChaseRadius);
        }
    }
}
