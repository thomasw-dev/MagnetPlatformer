using DG.Tweening;
using TMPro;
using UnityEngine;

public class MagneticObjectAlterChargeTimer : MonoBehaviour
{
    [SerializeField] MagneticObjectController _magneticObject;

    TMP_Text timerTMP;
    float _countdown;

    void Awake()
    {
        timerTMP = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        _magneticObject.StateController.EnumToState(MagneticObjectController.StateEnum.AlteredCharge).OnEnter += StartCountdown;
    }

    void Update()
    {
        if (_magneticObject.StateController.CurrentEnum == MagneticObjectController.StateEnum.AlteredCharge)
        {
            float displayText = Mathf.Round(_countdown * 10f) / 10f;
            timerTMP.text = displayText.ToString("F1");
        }
        else
        {
            timerTMP.text = "";
        }
    }

    void StartCountdown()
    {
        float duration = _magneticObject.Duration;
        DOTween.To(() => duration, x => duration = x, 0f, 5f)
            .SetEase(Ease.Linear) // Optional: Set the easing function
            .OnUpdate(() => _countdown = duration);
    }
}