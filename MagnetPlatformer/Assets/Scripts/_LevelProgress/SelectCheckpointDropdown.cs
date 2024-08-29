using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectCheckpointDropdown : MonoBehaviour
{
    [SerializeField] CheckpointManager _checkpointManager;

    LevelProgress _levelProgress;
    TMP_Dropdown _dropdownTMP;

    void Awake()
    {
        _levelProgress = _checkpointManager.GetLevelProgress();
        _dropdownTMP = GetComponent<TMP_Dropdown>();
    }

    void OnEnable()
    {
        if (_levelProgress == null) { return; }
        _levelProgress.OnSaveCheckpointIndex += UpdateCheckpointValue;
    }

    void OnDisable()
    {
        if (_levelProgress == null) { return; }
        _levelProgress.OnSaveCheckpointIndex -= UpdateCheckpointValue;
    }

    void Start()
    {
        ResizeDropdownOptions();
    }

    void ResizeDropdownOptions()
    {
        if (_checkpointManager == null) { return; }

        _dropdownTMP.ClearOptions();

        for (int i = 0; i < _checkpointManager.GetCheckpointArraySize(); i++)
        {
            string optionText = "Checkpoint " + i + (i == 0 ? " (Start)" : "");
            _dropdownTMP.options.Add(new TMP_Dropdown.OptionData(optionText));
        }

        _dropdownTMP.RefreshShownValue();
    }

    void UpdateCheckpointValue(int i) => _dropdownTMP.value = i;

    public void SaveSelectedValueToLevelProgress()
    {
        if (_levelProgress == null) { return; }
        _levelProgress.CurrentCheckpoint = _dropdownTMP.value;
    }
}
