using System;
using UnityEngine;

public class EnemyBossHealth : MonoBehaviour
{
    [Header("Values")]

    [SerializeField] int _lifeRemaining = 3;
    public int LifeRemaining
    {
        get => _lifeRemaining;
        private set
        {
            if (value < _lifeRemaining) OnLifeDecrease?.Invoke();
            _lifeRemaining = value;
        }
    }
    public event Action OnLifeDecrease;

    [SerializeField] int _currentHealth = 4;
    public int CurrentHealth
    {
        get => _currentHealth;
        private set
        {
            if (value < _currentHealth) OnHealthDecrease?.Invoke();
            _currentHealth = value;
        }
    }
    public event Action OnHealthDecrease;

    public void DealDamage()
    {
        CurrentHealth--;
    }

    public void DeductLife()
    {
        LifeRemaining--;
    }
}
