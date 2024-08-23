using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SceneEntryEffect : MonoBehaviour
{
    RawImage _rawImage;
    float _alpha = 1f;
    [SerializeField] float _duration = 1f;

    void Awake()
    {
        _rawImage = GetComponent<RawImage>();
    }

    void Start()
    {
        if (_rawImage == null) { return; }

        DOTween.To(x => _alpha = x, 1, 0, _duration)
        .SetEase(Ease.OutCirc)
        .OnUpdate(() =>
        {
            _rawImage.color = new Color(_rawImage.color.r, _rawImage.color.g, _rawImage.color.b, _alpha);
        })
        .OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
