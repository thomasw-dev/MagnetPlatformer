using UnityEngine;

public class Log : MonoBehaviour
{
    public static bool GameState;
    [SerializeField] bool _gameState;

    public static bool MagnetWeaponHit;
    [SerializeField] bool _magnetWeaponHit;

    public static bool EnemyBoss;
    [SerializeField] bool _enemyBoss;

    void OnValidate()
    {
        GameState = _gameState;
        MagnetWeaponHit = _magnetWeaponHit;
        EnemyBoss = _enemyBoss;
    }
}
