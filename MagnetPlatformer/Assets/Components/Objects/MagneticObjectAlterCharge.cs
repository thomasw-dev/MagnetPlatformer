using UnityEngine;

public class MagneticObjectAlterCharge : MonoBehaviour
{
    [Range(0f, 10f)]
    [SerializeField] float _duration = 1f;

    Magnet.Charge _initialCharge;

    void Start()
    {
        _initialCharge = GetComponent<MagneticObject>().CurrentCharge;
    }

    void Update()
    {
        
    }
}
