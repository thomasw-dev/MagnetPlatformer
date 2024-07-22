using UnityEngine;

[RequireComponent(typeof(MagneticInteractionValues))]
public class MagneticInteractionGizmos : MonoBehaviour
{
    void OnDrawGizmosSelected()
    {
        MagneticInteractionValues Values = GetComponent<MagneticInteractionValues>();

        if (Values.EmissionRadius)
        {
            /*switch (CurrentCharge)
            {
                case Magnet.Charge.Neutral: return;
                case Magnet.Charge.Positive: Gizmos.color = Color.red; break;
                case Magnet.Charge.Negative: Gizmos.color = Color.blue; break;
            }*/
            Gizmos.DrawWireSphere(transform.position, Values.Radius);
        }
    }
}
