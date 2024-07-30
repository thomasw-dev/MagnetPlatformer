using UnityEngine;

public class MagnetGunValues : MonoBehaviour
{
    public const int AMMO_MAX = 6;

    [Range(0, AMMO_MAX)]
    public int Ammo = AMMO_MAX;

    [Header("Cooldown")]
    public const float COOLDOWN_DURATION = 1.0f;
    [Range(0, COOLDOWN_DURATION)]
    public float CooldownDuration = 1.0f;

    [Header("Refill")]
    public const float REFILL_ONE_DURATION = 0.5f;
    [Range(0, REFILL_ONE_DURATION)]
    public float RefillOneDuration = REFILL_ONE_DURATION;

    [Header("Options")]
    public bool CostAmmoOnMagneticOnly = true;
}
