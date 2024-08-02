using UnityEngine;

public class MagneticInteractionVisual : MonoBehaviour
{
    [Header("Dependencies")]
    // These fields are required to be assigned in order for this module to function.

    [SerializeField] SpriteRenderer[] _spriteRenderers;

    [Header("Object Sprites")]
    [Tooltip("If assigned, this script will assign object sprites to the Sprite. If not, it will only change the Sprite color.")]
    [SerializeField] MagneticObjectSpriteSetCollection _objectSpritesCollection;

    [Header("Subscription")]
    [SerializeField] bool _subscribeToChanges = false;
    bool _isSubscribed = false;

    static Color NEUTRAL_COLOR = new Color(1, 1, 1, 1);
    static Color POSITIVE_COLOR = new Color(1, 0, 0, 1);
    static Color NEGATIVE_COLOR = new Color(0, 0, 1, 1);

    MagneticInteractionController GetController() => transform.parent.GetComponent<MagneticInteractionController>();

    bool DependenciesNullCheck()
    {
        bool pass = true;
        if (_spriteRenderers.Length == 0)
        {
            Debug.LogError($"Dependency missing: SpriteRenderer is not assigned.", this);
            pass = false;
        }
        return pass;
    }

    void OnValidate()
    {
        // Subscribe
        if (_subscribeToChanges && !_isSubscribed)
        {
            if (!DependenciesNullCheck())
            {
                // Cannot subscribe, revert to false
                _subscribeToChanges = false;
            }
            else
            {
                SubscribeToChanges();

                // Update instantly
                UpdateSpriteByCharge(GetController().CurrentCharge);
                UpdateSpriteSetByType(GetController().CurrentType);
            }
        }
        // Unsubscribe
        else if (!_subscribeToChanges && _isSubscribed)
        {
            UnsubscribeToChanges();
        }
    }

    void OnEnable()
    {
        if (!DependenciesNullCheck()) { return; }

        SubscribeToChanges();
    }

    void OnDisable()
    {
        if (!DependenciesNullCheck()) { return; }

        UnsubscribeToChanges();
    }

    void SubscribeToChanges()
    {
        GetController().OnCurrentChargeChanged += UpdateSpriteByCharge;
        GetController().OnCurrentTypeChanged += UpdateSpriteSetByType;
        _isSubscribed = true;
    }

    void UnsubscribeToChanges()
    {
        GetController().OnCurrentChargeChanged -= UpdateSpriteByCharge;
        GetController().OnCurrentTypeChanged -= UpdateSpriteSetByType;
        _isSubscribed = false;
    }

    void UpdateSpriteSetByType(MagneticObject.Type type)
    {
        if (_objectSpritesCollection == null) { return; }
        foreach (var spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.sprite = _objectSpritesCollection.GetSpriteSetByType(type).GetSpriteByCharge(GetController().CurrentCharge);
        }
    }

    void UpdateSpriteByCharge(Magnet.Charge charge)
    {
        if (_objectSpritesCollection != null)
        {
            foreach (var spriteRenderer in _spriteRenderers)
            {
                spriteRenderer.sprite = _objectSpritesCollection.GetSpriteSetByType(GetController().CurrentType).GetSpriteByCharge(charge);
            }
        }
        else
        {
            ChangeColorByCharge(charge);
        }
    }

    void ChangeColorByCharge(Magnet.Charge charge)
    {
        foreach (var spriteRenderer in _spriteRenderers)
        {
            if (charge == Magnet.Charge.Neutral) spriteRenderer.color = NEUTRAL_COLOR;
            if (charge == Magnet.Charge.Positive) spriteRenderer.color = POSITIVE_COLOR;
            if (charge == Magnet.Charge.Negative) spriteRenderer.color = NEGATIVE_COLOR;
        }
    }
}
