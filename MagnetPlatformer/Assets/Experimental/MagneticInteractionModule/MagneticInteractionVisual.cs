using UnityEngine;

public class MagneticInteractionVisual : MonoBehaviour
{
    [Header("Dependencies")]
    // These fields are required to be assigned in order for this module to function.

    [SerializeField] SpriteRenderer _spriteRenderer;

    [Header("Subscription")]
    [SerializeField] bool _updateToCurrentCharge = false;
    bool _isSubscribed = false;

    static Color NEUTRAL_COLOR = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    static Color POSITIVE_COLOR = new Color(0.67f, 0, 0, 1.0f);
    static Color NEGATIVE_COLOR = new Color(0, 0, 0.67f, 1.0f);

    MagneticInteractionController GetController() => GetComponent<MagneticInteractionController>();


    bool DependenciesNullCheck()
    {
        bool pass = true;
        if (_spriteRenderer == null)
        {
            Debug.LogError($"Dependency missing: SpriteRenderer is not assigned.", this);
            pass = false;
        }
        return pass;
    }

    void OnValidate()
    {
        // Subscribe
        if (_updateToCurrentCharge && !_isSubscribed)
        {
            if (!DependenciesNullCheck())
            {
                // Cannot subscribe, revert to false
                _updateToCurrentCharge = false;
            }
            else
            {
                GetController().OnCurrentChargeChanged += SetColorByCharge;
                _isSubscribed = true;

                // Call it once to update it instantly
                SetColorByCharge(GetController().CurrentCharge);
            }
        }
        // Unsubscribe
        else if (!_updateToCurrentCharge && _isSubscribed)
        {
            GetController().OnCurrentChargeChanged -= SetColorByCharge;
            _isSubscribed = false;
        }
    }

    void OnEnable()
    {
        if (!DependenciesNullCheck()) { return; }

        GetController().OnCurrentChargeChanged += SetColorByCharge;
    }

    void OnDisable()
    {
        if (!DependenciesNullCheck()) { return; }

        GetController().OnCurrentChargeChanged -= SetColorByCharge;
    }

    void SetColorByCharge(Magnet.Charge charge)
    {
        if (charge == Magnet.Charge.Neutral) _spriteRenderer.color = NEUTRAL_COLOR;
        if (charge == Magnet.Charge.Positive) _spriteRenderer.color = POSITIVE_COLOR;
        if (charge == Magnet.Charge.Negative) _spriteRenderer.color = NEGATIVE_COLOR;
    }
}
