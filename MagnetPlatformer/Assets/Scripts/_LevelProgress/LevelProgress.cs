using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Progress", order = Constants.LEVEL_PROGRESS)]
public class LevelProgress : ScriptableObject
{
    public int CurrentCheckpoint = 0;

    public event Action<int> OnSaveCheckpointIndex;

    public void SaveCheckpointIndex(int i)
    {
        CurrentCheckpoint = i;

        if (Log.Checkpoint)
        {
            Debug.Log($"Checkpoint Saved (Index = {i}).");
        }

        OnSaveCheckpointIndex?.Invoke(CurrentCheckpoint);
    }

    public void ResetProgress()
    {
        CurrentCheckpoint = 0;
    }
}
