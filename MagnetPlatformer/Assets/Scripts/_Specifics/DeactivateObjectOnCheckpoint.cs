using UnityEngine;

public class DeactivateObjectOnCheckpoint : MonoBehaviour
{
    [SerializeField] GameObject _object;
    [SerializeField] LevelProgress _levelProgress;
    [SerializeField] int _deactivateOnCheckpoint = 0;

    void Update()
    {
        bool state = _levelProgress.CurrentCheckpoint == _deactivateOnCheckpoint ? false : true;
        _object.SetActive(state);
    }
}
