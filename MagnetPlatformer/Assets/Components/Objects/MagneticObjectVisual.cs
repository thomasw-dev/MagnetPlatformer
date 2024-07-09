using UnityEngine;

public class MagneticObjectVisual : MonoBehaviour
{
    static Color _neutralColor = new Color(1.0f, 1.0f, 1.0f, 1.0f); 
    static Color _positiveColor = new Color(0.67f, 0, 0, 1.0f);
    static Color _negativeColor = new Color(0, 0, 0.67f, 1.0f);

    [SerializeField] MagneticObject _magneticObject;

    SpriteRenderer _spriteRenderer;

    void Awake()
    {
        NullCheck();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void NullCheck()
    {
        if (_magneticObject == null)
        {
            Debug.LogError("MagneticObjectVisual is not assigned a MagneticObject.", this);
        }
    }

    void OnEnable()
    {
        GameState.Initialize.OnEnter += Initialize;
        _magneticObject.OnCurrentChargeChanged += SetVisual;
    }

    void OnDisable()
    {
        GameState.Initialize.OnEnter -= Initialize;
        _magneticObject.OnCurrentChargeChanged -= SetVisual;
    }

    void Initialize()
    {
        SetVisual(_magneticObject.CurrentCharge);
    }

    void SetVisual(Magnet.Charge charge)
    {
        if (charge == Magnet.Charge.Neutral) _spriteRenderer.color = _neutralColor;
        if (charge == Magnet.Charge.Positive) _spriteRenderer.color = _positiveColor;
        if (charge == Magnet.Charge.Negative) _spriteRenderer.color = _negativeColor;
    }
}
