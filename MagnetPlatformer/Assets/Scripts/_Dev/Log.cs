using UnityEngine;

public class Log : MonoBehaviour
{
    public static bool GameState;
    [SerializeField] bool _gameState;

    public static bool MagnetWeaponHit;
    [SerializeField] bool _magnetWeaponHit;

    public static bool MagneticForceOnSelected;
    [SerializeField] bool _magneticForceOnSelected;

    void OnValidate()
    {
        GameState = _gameState;
        MagnetWeaponHit = _magnetWeaponHit;
        MagneticForceOnSelected = _magneticForceOnSelected;
    }
}
