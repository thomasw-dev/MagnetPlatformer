using UnityEngine;

public class MagneticObjectVisual : MonoBehaviour
{
    [SerializeField] MagneticObject _magneticObject;
    [SerializeField] MagnetSpriteVariants _magneticSpriteVariants;

    SpriteRenderer _spriteRenderer;

    void Awake()
    {
        if (!NullCheckPass()) { return; }
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    bool NullCheckPass()
    {
        if (_magneticObject == null)
        {
            Debug.LogError("MagneticObjectVisual is not assigned a MagneticObject.", this);
            return false;
        }
        return true;
    }

    void OnEnable()
    {
        GameState.Initialize.OnEnter += Initialize;
        _magneticObject.OnCurrentChargeChanged += SetSprite;
    }

    void OnDisable()
    {
        GameState.Initialize.OnEnter -= Initialize;
        _magneticObject.OnCurrentChargeChanged -= SetSprite;
    }

    void Initialize()
    {
        SetSprite(_magneticObject.CurrentCharge);
    }

    void SetSprite(Magnet.Charge charge)
    {
        Debug.Log($"Updating the sprite {charge}");
        if (charge == Magnet.Charge.Neutral) _spriteRenderer.sprite = _magneticSpriteVariants.Neutral;
        if (charge == Magnet.Charge.Positive) _spriteRenderer.sprite = _magneticSpriteVariants.Positive;
        if (charge == Magnet.Charge.Negative) _spriteRenderer.sprite = _magneticSpriteVariants.Negative;
    }

    #region Inspector

    public void UpdateSprite(Magnet.Charge charge)
    {
        SetSprite(GetComponent<SpriteRenderer>(), charge);
    }

    void SetSprite(SpriteRenderer spriteRenderer, Magnet.Charge charge)
    {
        if (charge == Magnet.Charge.Neutral) spriteRenderer.sprite = _magneticSpriteVariants.Neutral;
        if (charge == Magnet.Charge.Positive) spriteRenderer.sprite = _magneticSpriteVariants.Positive;
        if (charge == Magnet.Charge.Negative) spriteRenderer.sprite = _magneticSpriteVariants.Negative;
    }

    #endregion
}
