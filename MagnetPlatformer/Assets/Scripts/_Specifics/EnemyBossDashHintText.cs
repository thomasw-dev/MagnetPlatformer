using DG.Tweening;
using TMPro;
using UnityEngine;

public class EnemyBossDashHintText : MonoBehaviour
{
    [SerializeField] EnemyBossDash _enemyBossDash;
    [SerializeField] float duration = 1f;
    [SerializeField] float flashSpeed = 0.1f;
    bool _flashingEffectStarted = false;

    TMP_Text _tmpText;

    void Awake()
    {
        _tmpText = GetComponent<TextMeshProUGUI>();
    }

    void OnDisable()
    {
        InitialState();
    }

    void Start()
    {
        InitialState();
    }

    void Update()
    {
        if (_enemyBossDash.IsCountingDown && _enemyBossDash.NextDashIn <= duration)
        {
            if (!_flashingEffectStarted)
            {
                StartFlashingEffect();
                _flashingEffectStarted = true;
            }
        }
        else
        {
            InitialState();
            _flashingEffectStarted = false;
        }
    }

    void StartFlashingEffect()
    {
        _tmpText.color = new Color(_tmpText.color.r, _tmpText.color.g, _tmpText.color.b, 1f);
        _tmpText.DOFade(0.0f, flashSpeed).SetLoops(-1, LoopType.Yoyo);
    }

    void InitialState()
    {
        _tmpText.DOKill();
        _tmpText.color = new Color(_tmpText.color.r, _tmpText.color.g, _tmpText.color.b, 0f);
    }
}
