using TMPro;
using UnityEngine;

public class SelectCheckpointDropdown : MonoBehaviour
{
    [SerializeField] LevelProgress _levelProgress;

    TMP_Dropdown _dropdownTMP;
    [SerializeField] int _index;

    void Awake()
    {
        _dropdownTMP = GetComponent<TMP_Dropdown>();
    }

    void Update()
    {
        _index = _dropdownTMP.value;
    }

    public void SaveSelectedValueToLevelProgress()
    {
        _levelProgress.CurrentCheckpoint = _index;
    }
}
