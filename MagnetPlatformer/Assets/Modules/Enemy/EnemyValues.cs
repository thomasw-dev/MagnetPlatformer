using System;
using UnityEngine;

public class EnemyValues : MonoBehaviour
{
    [Header("Movement")]
    [Range(1f, 50f)]
    public float ChaseAcceleration = 10f;
    [Range(1f, 10f)]
    public float WalkAcceleration = 5f;

    [Header("Options")]
    public bool ReturnToInitialPosition = true;
}
