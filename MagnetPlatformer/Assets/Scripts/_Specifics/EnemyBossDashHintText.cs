using DG.Tweening;
using TMPro;
using UnityEngine;

public class EnemyBossDashHintText : MonoBehaviour
{
    [SerializeField] EnemyBossDash _enemyBossDash;

    [SerializeField] float flashSpeed = 0.1f;
    bool _flashingEffectStarted = false;

    TMP_Text _tmpText;

    void Awake()
    {
        _tmpText = GetComponent<TextMeshProUGUI>();
    }

    void OnDisable()
    {
        _tmpText.DOKill();
    }

    void Update()
    {
        if (_enemyBossDash.ReadyToDash && Time.time - _enemyBossDash.TimeToDash <= 1f)
        {
            if (!_flashingEffectStarted)
            {
                StartFlashingEffect();
                _flashingEffectStarted = true;
            }
        }
        else
        {
            _tmpText.enabled = false;
            _flashingEffectStarted = false;
        }
    }

    void StartFlashingEffect()
    {
        _tmpText.enabled = true;
        _tmpText.DOFade(0.0f, flashSpeed).SetLoops(-1, LoopType.Yoyo);
    }
}
