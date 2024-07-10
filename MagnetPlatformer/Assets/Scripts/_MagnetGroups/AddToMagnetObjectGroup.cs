using UnityEngine;

public class AddToMagnetObjectGroup : MonoBehaviour
{
    public MagnetObjectGroup MagnetObjectGroup;

    private void OnEnable()
    {
        MagnetObjectGroup.Add(this.gameObject);
    }

    private void OnDisable()
    {
        MagnetObjectGroup.Remove(this.gameObject);
    }
}