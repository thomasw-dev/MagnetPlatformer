using System.Collections;
using UnityEngine;

public class MagneticObjectVisual : MonoBehaviour
{
    [SerializeField] MagneticObjectController _magneticObject;
    [SerializeField] MagnetSpriteSetCollection _spriteCollection;
    [SerializeField] Sprite _alteredChargeTriggerEffect;

    const float ALTERED_CHARGE_TRIGGER_EFFECT_DURATION = 0.06f;

    SpriteRenderer _spriteRenderer;
    bool _constantChargeEffectEnabled = false;
    Color _spriteRendererColorPrev;

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
        _magneticObject.OnChargeIsConstantChanged += ToggleConstantChargeEffect;
        _magneticObject.StateController.EnumToState(MagneticObjectController.EnumState.Normal).OnEnter += SetNormalSpriteColor;
        _magneticObject.StateController.EnumToState(MagneticObjectController.EnumState.AlteredCharge).OnEnter += AlteredChargeTriggerEffect;
    }

    void OnDisable()
    {
        GameState.Initialize.OnEnter -= Initialize;
        _magneticObject.OnCurrentChargeChanged -= SetSprite;
        _magneticObject.OnChargeIsConstantChanged -= ToggleConstantChargeEffect;
    }

    void Initialize()
    {
        SetSprite(_magneticObject.CurrentCharge);
    }

    void SetSprite(Magnet.Charge charge)
    {
        MagneticObject.Type type = _magneticObject.gameObject.GetComponent<MagneticObjectSetup>().Type;
        _spriteRenderer.sprite = _spriteCollection.GetSpriteSetByType(type).GetSpriteByCharge(charge);

    }

    void ToggleConstantChargeEffect(bool state)
    {
        _constantChargeEffectEnabled = state;
        if (_constantChargeEffectEnabled)
        {
            _spriteRendererColorPrev = _spriteRenderer.color;
            Color.RGBToHSV(_spriteRenderer.color, out float currentHue, out float saturation, out float currentValue);
            _spriteRenderer.color = Color.HSVToRGB(currentHue, 1, currentValue);
        }
        else
        {
            _spriteRenderer.color = _spriteRendererColorPrev;
        }
    }

    void AlteredChargeTriggerEffect() => StartCoroutine(DoAlteredChargeTriggerEffect());
    IEnumerator DoAlteredChargeTriggerEffect()
    {
        Sprite currentSprite = _spriteRenderer.sprite;
        _spriteRenderer.sprite = _alteredChargeTriggerEffect;
        _spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(ALTERED_CHARGE_TRIGGER_EFFECT_DURATION);
        _spriteRenderer.sprite = currentSprite;

        if (_magneticObject.CurrentCharge == Magnet.Charge.Positive)
        {
            _spriteRenderer.color = Color.HSVToRGB(0, 0.67f, 1f);
        }
        if (_magneticObject.CurrentCharge == Magnet.Charge.Negative)
        {
            _spriteRenderer.color = Color.HSVToRGB(0.67f, 0.67f, 1f);
        }
    }

    void SetNormalSpriteColor()
    {
        _spriteRenderer.color = Color.white;
    }

    #region Inspector

    public void UpdateSprite(Magnet.Charge charge)
    {
        SetSprite(GetComponent<SpriteRenderer>(), charge);
    }

    void SetSprite(SpriteRenderer spriteRenderer, Magnet.Charge charge)
    {
        MagneticObject.Type type = _magneticObject.gameObject.GetComponent<MagneticObjectSetup>().Type;
        spriteRenderer.sprite = _spriteCollection.GetSpriteSetByType(type).GetSpriteByCharge(charge);
    }

    #endregion
}
