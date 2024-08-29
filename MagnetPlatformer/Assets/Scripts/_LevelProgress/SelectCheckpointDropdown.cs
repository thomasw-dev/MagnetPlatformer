using TMPro;
using UnityEngine;

public class SelectCheckpointDropdown : MonoBehaviour
{
    [SerializeField] LevelProgress _levelProgress;

    TMP_Dropdown _dropdownTMP;

    void Awake()
    {
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

    void UpdateCheckpointValue(int i) => _dropdownTMP.value = i;

    public void SaveSelectedValueToLevelProgress()
    {
        if (_levelProgress == null) { return; }
        _levelProgress.CurrentCheckpoint = _dropdownTMP.value;
    }
}
