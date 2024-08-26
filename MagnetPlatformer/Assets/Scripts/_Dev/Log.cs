using UnityEngine;

public class Log : MonoBehaviour
{
    public static bool GameState;
    [SerializeField] bool _gameState;

    public static bool MagnetWeaponHit;
    [SerializeField] bool _magnetWeaponHit;

    public static bool EnemyBoss;
    [SerializeField] bool _enemyBoss;

    public static bool Checkpoint;
    [SerializeField] bool _checkpoint;

    void OnValidate()
    {
        GameState = _gameState;
        MagnetWeaponHit = _magnetWeaponHit;
        EnemyBoss = _enemyBoss;
        Checkpoint = _checkpoint;
    }
}
