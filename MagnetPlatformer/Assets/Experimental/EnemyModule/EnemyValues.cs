using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyValues : MonoBehaviour
{
    [Range(1f, 10f)]
    [SerializeField] float _moveSpeed = 1.0f;
}
