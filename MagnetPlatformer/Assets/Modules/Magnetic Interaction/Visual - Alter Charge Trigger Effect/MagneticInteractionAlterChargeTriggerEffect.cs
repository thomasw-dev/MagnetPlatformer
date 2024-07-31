using DG.Tweening;
using UnityEngine;

public class MagneticInteractionAlterChargeTriggerEffect : MonoBehaviour
{
    [Header("Dependencies")] // Required to be assigned in the Inspector
    [SerializeField] MagneticInteractionController _magneticInteractionController;

    SpriteRenderer _spriteRenderer;
    float alpha;
    Tweener _fadeTween;

    const float FADE_DURATION = 0.3f;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnValidate()
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
