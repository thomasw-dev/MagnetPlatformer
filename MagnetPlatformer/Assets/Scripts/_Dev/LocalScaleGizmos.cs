using UnityEngine;

public class LocalScaleGizmos : MonoBehaviour
{
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
