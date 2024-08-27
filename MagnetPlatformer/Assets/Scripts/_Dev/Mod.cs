using UnityEngine;

public class Mod : MonoBehaviour
{
    public static bool EnableWin;
    [SerializeField] bool _enableWin;

    public static bool EnableDeath;
    [SerializeField] bool _enableDeath;

    void WriteValues()
    {
        EnableWin = _enableWin;
        EnableDeath = _enableDeath;
    }

    void OnEnable() => WriteValues();

    void OnValidate() => WriteValues();
}
