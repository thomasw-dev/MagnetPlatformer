using System;
using UnityEngine;

public class EnemyValues : MonoBehaviour
{
    [Header("Movement")]
    [Range(1f, 10f)]
    public float Acceleration = 1.0f;

    [Header("Chase")]
    [Range(1f, 20f)]
    public float ChaseRadius = 10f;

    public event Action<float> SetChaseRadius;

    [Header("Options")]
    public bool ReturnToInitialPosition = true;

    void OnValidate()
    {
        SetChaseRadius?.Invoke(ChaseRadius);
    }
}
