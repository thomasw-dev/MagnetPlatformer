using DG.Tweening;
using UnityEngine;

public class MagneticInteractionAlterChargeTriggerEffect : MonoBehaviour
{
    [Header("Dependencies")] // Required to be assigned in the Inspector
    [SerializeField] MagneticInteractionController _magneticInteractionController;
    [SerializeField] SpriteRenderer _maskSourceSpriteRenderer;

    SpriteRenderer _spriteRenderer;
    SpriteMask _spriteMask;
    float alpha;
    Tweener _fadeTween;

    const float FADE_DURATION = 0.3f;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteMask = GetComponent<SpriteMask>();
    }

    void OnValidate()
    {
        if (_magneticInteractionController != null)
        {
            _magneticInteractionController.OnAlterCharge += AlteredChargeTriggerEffect;
        }
    }

    void OnEnable()
    {
        if (_magneticInteractionController != null)
        {
            _magneticInteractionController.OnAlterCharge += AlteredChargeTriggerEffect;
        }
    }

    void OnDisable()
    {
        if (_magneticInteractionController != null)
        {
            _magneticInteractionController.OnAlterCharge -= AlteredChargeTriggerEffect;
        }
    }

    void Update()
    {
        _spriteMask.sprite = _maskSourceSpriteRenderer.sprite;
    }

    void AlteredChargeTriggerEffect(Magnet.Charge charge)
    {
        // Kill any current tween progress
        if (_fadeTween != null && _fadeTween.IsActive()) _fadeTween.Kill();

        // Start the tween again
        StartFadeTween();
    }

    void StartFadeTween()
    {
        _fadeTween = DOTween.To(x => alpha = x, 1, 0, FADE_DURATION).SetEase(Ease.OutCirc)
            .OnPlay(() =>
            {

            })
            .OnUpdate(() =>
            {
                _spriteRenderer.color = new Color(1, 1, 1, alpha);
            })
            .OnComplete(() =>
            {

            });

        _fadeTween.Play();
    }
}
