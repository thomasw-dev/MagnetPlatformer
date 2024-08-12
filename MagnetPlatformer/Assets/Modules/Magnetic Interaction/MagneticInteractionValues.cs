using System;
using UnityEngine;

public class MagneticInteractionValues : MonoBehaviour
{
    [Header("Charge")]

    [SerializeField] bool _neutral = true;
    [SerializeField] bool _positive = false;
    [SerializeField] bool _negative = false;

    [Header("Movement Constrain")]

    [SerializeField] MagneticObject.Type _type;

    [Header("Emission")]

    [Range(0, 10f)]
    public float EmissionForce = 5f;

    public float EmissionRadius = 10f;

    [Header("Alter Charge")]
    [Range(0f, 10f)]
    public float Duration = 5f;

    MagneticInteractionController GetController() => GetComponent<MagneticInteractionController>();

    void Awake()
    {
        SetChargeByBools();
        UpdateChargeBools(GetController().CurrentCharge);
        UpdateType(_type);
    }

    void OnValidate()
    {
        SetChargeByBools();
        UpdateChargeBools(GetController().CurrentCharge);
        UpdateType(_type);
    }

    void UpdateType(MagneticObject.Type type)
    {
        GetController().CurrentType = type;
        UpdateRigidbody(GetController().CurrentType);
    }

    void UpdateRigidbody(MagneticObject.Type type)
    {
        Rigidbody2D rigidbody = GetController().Rigidbody;
        if (rigidbody == null) { return; }

        if (type == MagneticObject.Type.Free)
            SetRigidbodyParameters(RigidbodyType2D.Dynamic, RigidbodyConstraints2D.FreezeRotation);

        if (type == MagneticObject.Type.Vertical)
            SetRigidbodyParameters(RigidbodyType2D.Dynamic, RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX);

        if (type == MagneticObject.Type.Horizontal)
            SetRigidbodyParameters(RigidbodyType2D.Dynamic, RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY);

        if (type == MagneticObject.Type.Static)
            SetRigidbodyParameters(RigidbodyType2D.Static, RigidbodyConstraints2D.FreezeAll);

        void SetRigidbodyParameters(RigidbodyType2D bodyType, RigidbodyConstraints2D constraints)
        {
            rigidbody.bodyType = bodyType;
            rigidbody.constraints = constraints;
        }
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
