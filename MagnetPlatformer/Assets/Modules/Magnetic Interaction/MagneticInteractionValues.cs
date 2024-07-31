using UnityEngine;

public class MagneticInteractionValues : MonoBehaviour
{
    [Header("Charge")]

    [SerializeField] bool _neutral = true;
    [SerializeField] bool _positive = false;
    [SerializeField] bool _negative = false;

    [Header("Emission")]

    [Range(-MagneticInteractionPhysics.DISTANCE_MULTIPLIER, MagneticInteractionPhysics.DISTANCE_MULTIPLIER)]
    public float EmissionGain = 0f;

    public float EmissionRadius = 10f;

    MagneticInteractionController GetController() => GetComponent<MagneticInteractionController>();

    void OnValidate()
    {
        SetChargeByBools();
        UpdateChargeBools(GetController().CurrentCharge);
    }

    void SetChargeByBools()
    {
        Magnet.Charge currentCharge = GetController().CurrentCharge;

        if (currentCharge == Magnet.Charge.Neutral)
        {
            if (_positive && !_negative) GetController().CurrentCharge = Magnet.Charge.Positive;
            if (!_positive && _negative) GetController().CurrentCharge = Magnet.Charge.Negative;
        }

        else if (currentCharge == Magnet.Charge.Positive)
        {
            if (_neutral && !_negative) GetController().CurrentCharge = Magnet.Charge.Neutral;
            if (!_neutral && _negative) GetController().CurrentCharge = Magnet.Charge.Negative;
        }

        else if (currentCharge == Magnet.Charge.Negative)
        {
            if (_neutral && !_positive) GetController().CurrentCharge = Magnet.Charge.Neutral;
            if (!_neutral && _positive) GetController().CurrentCharge = Magnet.Charge.Positive;
        }
    }

    void UpdateChargeBools(Magnet.Charge charge)
    {
        _neutral = charge == Magnet.Charge.Neutral;
        _positive = charge == Magnet.Charge.Positive;
        _negative = charge == Magnet.Charge.Negative;
    }
}
