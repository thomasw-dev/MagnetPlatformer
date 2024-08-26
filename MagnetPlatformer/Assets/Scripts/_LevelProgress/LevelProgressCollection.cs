using UnityEngine;

public class LevelProgressCollection : MonoBehaviour
{
    [SerializeField] LevelProgress[] _allLevelProgress;

    [ContextMenu("Reset All")]
    public void ResetAll()
    {
        foreach (LevelProgress levelProgress in _allLevelProgress)
        {
            levelProgress.ResetProgress();
        }
    }
}
