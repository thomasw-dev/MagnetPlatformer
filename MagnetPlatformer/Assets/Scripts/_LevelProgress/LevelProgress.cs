using UnityEngine;

[CreateAssetMenu(fileName = "Level Progress", order = Constants.LEVEL_PROGRESS)]
public class LevelProgress : ScriptableObject
{
    public int CurrentCheckpoint = 0;

    public void SaveCheckpointIndex(int i)
    {
        CurrentCheckpoint = i;
        Debug.Log($"SaveCheckpointIndex: {i}");
    }

    public void ResetProgress()
    {
        CurrentCheckpoint = 0;
    }
}
