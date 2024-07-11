using UnityEngine;

public class Mod : MonoBehaviour
{
    public static bool EnableDeath;
    [SerializeField] bool _enableDeath;

    void OnValidate()
    {
        EnableDeath = _enableDeath;
    }
}
