using UnityEngine;

[CreateAssetMenu(fileName = "Level Progress", order = Constants.LEVEL_PROGRESS)]
public class LevelProgress : ScriptableObject
{
    public int CurrentCheckpoint = 0;

    public void SaveCheckpointIndex(int i)
    {
        CurrentCheckpoint = i;

        if (Log.Checkpoint)
        {
            Debug.Log($"Checkpoint Saved (Index = {i}).");
        }
    }

    public void ResetProgress()
    {
        CurrentCheckpoint = 0;
    }
}
