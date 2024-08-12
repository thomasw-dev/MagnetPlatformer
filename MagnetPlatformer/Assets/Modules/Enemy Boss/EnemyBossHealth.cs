using UnityEngine;

public class EnemyBossHealth : MonoBehaviour
{
    const int MAX_HEALTH = 10;
    public int CurrentHealth { get => _currentHealth; }
    [SerializeField] int _currentHealth = MAX_HEALTH;

    public void DealDamage()
    {
        _currentHealth--;
    }
}
