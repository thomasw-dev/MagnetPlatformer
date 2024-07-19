using UnityEngine;

[RequireComponent(typeof(MagneticInteractionConfig))]
public class MagneticInteractionGizmos : MonoBehaviour
{
    void OnDrawGizmosSelected()
    {
        MagneticInteractionConfig Config = GetComponent<MagneticInteractionConfig>();

        if (Config.EmissionRadius)
        {
            /*switch (CurrentCharge)
            {
                case Magnet.Charge.Neutral: return;
                case Magnet.Charge.Positive: Gizmos.color = Color.red; break;
                case Magnet.Charge.Negative: Gizmos.color = Color.blue; break;
            }*/
            Gizmos.DrawWireSphere(transform.position, Config.Radius);
        }
    }
}
