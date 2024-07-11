using UnityEngine;

namespace Experimental
{
    public class TargetGizmos : MonoBehaviour
    {
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, SmoothMagnet.MAX_DISTANCE);
        }
    }
}