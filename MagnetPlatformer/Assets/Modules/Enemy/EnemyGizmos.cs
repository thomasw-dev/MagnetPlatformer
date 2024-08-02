using UnityEngine;

public class EnemyGizmos : MonoBehaviour
{
    const float TARGET_POINT_SIZE = 0.2f;
    static Color TARGET_POINT_COLOR = Color.yellow;

    EnemyController GetController() => transform.parent.GetComponent<EnemyController>();

    void OnDrawGizmos()
    {
        if (DebugManager.EnemySettings.TargetPoint)
        {
            Gizmos.color = TARGET_POINT_COLOR;
            Gizmos.DrawWireSphere(GetController().TargetPoint, TARGET_POINT_SIZE);
        }
    }
}
