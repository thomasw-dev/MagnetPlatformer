using UnityEngine;

public class EnemyBossGizmos : MonoBehaviour
{
    const float MOVE_TARGET_SIZE = 0.2f;
    static Color MOVE_TARGET_COLOR = Color.yellow;

    EnemyBossController GetController() => transform.parent.GetComponent<EnemyBossController>();

    void OnDrawGizmos()
    {
        if (GizmosSettings.EnemyBoss.MoveTarget)
        {
            Gizmos.color = MOVE_TARGET_COLOR;
            Gizmos.DrawWireSphere(GetController().MoveTarget.position, MOVE_TARGET_SIZE);
        }
    }
}
