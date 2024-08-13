using System;
using UnityEngine;

public class EnemyValues : MonoBehaviour
{
    [Header("Movement")]
    public float ChaseAcceleration = 10f;
    public float WalkAcceleration = 5f;

    [Header("Options")]
    public bool ReturnToInitialPosition = true;
}
