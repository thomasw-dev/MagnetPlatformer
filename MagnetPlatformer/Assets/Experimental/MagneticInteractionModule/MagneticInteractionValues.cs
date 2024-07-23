using UnityEngine;

public class MagneticInteractionValues : MonoBehaviour
{
    [Header("Charge")]

    [SerializeField] bool _neutral = true;
    [SerializeField] bool _positive = false;
    [SerializeField] bool _negative = false;

    [Header("Emission")]

    public bool EmitForce = true;

    [Range(0f, 1000f)]
    public float Force = 1f;

    [Range(0f, 50f)]
    public float Radius = 10f;

    [Header("Others")]

    public bool UseGravity = true;

    void OnValidate()
    {
        SetChargeByBools();
        UpdateChargeBools(GetController().CurrentCharge);
    }

    MagneticInteractionController GetController() => GetComponent<MagneticInteractionController>();

    void SetChargeByBools()
    {
        Magnet.Charge currentCharge = GetController().CurrentCharge;
        Magnet.Charge setCharge = Magnet.Charge.Neutral;

        if (currentCharge == Magnet.Charge.Neutral)
        {
            if (_positive && !_negative) setCharge = Magnet.Charge.Positive;
            if (!_positive && _negative) setCharge = Magnet.Charge.Negative;
        }

        else if (currentCharge == Magnet.Charge.Positive)
        {
            if (_neutral && !_negative) setCharge = Magnet.Charge.Neutral;
            if (!_neutral && _negative) setCharge = Magnet.Charge.Negative;
        }

        else if (currentCharge == Magnet.Charge.Negative)
        {
            if (_neutral && !_positive) setCharge = Magnet.Charge.Neutral;
            if (!_neutral && _positive) setCharge = Magnet.Charge.Positive;
        }

        GetController().CurrentCharge = setCharge;
    }

    void UpdateChargeBools(Magnet.Charge charge)
    {
        _neutral = charge == Magnet.Charge.Neutral;
        _positive = charge == Magnet.Charge.Positive;
        _negative = charge == Magnet.Charge.Negative;
    }
}
