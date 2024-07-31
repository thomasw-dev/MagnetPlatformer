using TMPro;
using UnityEngine;

public class MagneticInteractionAlterChargeTimeDisplay : MonoBehaviour
{
    [Header("Dependencies")] // Required to be assigned in the Inspector
    [SerializeField] MagneticInteractionController _magneticInteractionController;

    TMP_Text timerTMP;

    void Awake()
    {
        timerTMP = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (_magneticInteractionController == null) { return; }

        float timeRemaining = _magneticInteractionController.AlterChargeTimeRemaining;
        if (timeRemaining > 0)
        {
            float displayText = Mathf.Round(timeRemaining * 10f) / 10f;
            timerTMP.text = displayText.ToString("F1");
        }
        else
        {
            timerTMP.text = "";
        }
    }
}
