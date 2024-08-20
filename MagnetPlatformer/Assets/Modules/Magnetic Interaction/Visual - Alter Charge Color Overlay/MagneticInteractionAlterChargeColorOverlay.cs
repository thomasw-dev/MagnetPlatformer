using UnityEngine;
using UnityEngine.UIElements;

public class MagneticInteractionAlterChargeColorOverlay : MonoBehaviour
{
    [Header("Dependencies")] // Required to be assigned in the Inspector
    [SerializeField] Transform _rootParent;
    [SerializeField] MagneticInteractionController _magneticInteractionController;
    [SerializeField] SpriteRenderer _sourceSpriteRenderer;

    [Header("Tunables")]
    [Range(0, 1)]
    [SerializeField] float alpha = 0.5f;

    SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (_magneticInteractionController == null) { return; }

        Color sourceColor = _sourceSpriteRenderer.color;

        if (_magneticInteractionController.CurrentCharge == _magneticInteractionController.InitialCharge)
        {
            // Scale (Y: 0)
            transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);

            // Source sprite color (full)
            _sourceSpriteRenderer.color = new Color(sourceColor.r, sourceColor.g, sourceColor.b, 1);
        }
        else
        {
            // Scale (Y: shrink scale)
            float timeRemaining = _magneticInteractionController.AlterChargeTimeRemaining;
            float duration = _magneticInteractionController.Values.Duration;
            float scale = timeRemaining / duration;
            transform.localScale = new Vector3(transform.localScale.x, scale, transform.localScale.z);

            // Source sprite color (transparent)
            _sourceSpriteRenderer.color = new Color(sourceColor.r, sourceColor.g, sourceColor.b, alpha);

            // Overlay sprite color
            Magnet.Charge charge = _magneticInteractionController.CurrentCharge;
            _spriteRenderer.color = GetSpriteColorByCharge(charge);
        }
    }

    Color GetSpriteColorByCharge(Magnet.Charge charge)
    {
        return charge switch
        {
            Magnet.Charge.Neutral => new Color(1, 1, 1, alpha),
            Magnet.Charge.Positive => new Color(1, 0, 0, alpha),
            Magnet.Charge.Negative => new Color(0, 0, 1, alpha),
            _ => new Color()
        };
    }
}
