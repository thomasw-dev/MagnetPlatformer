using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject _player;

    void Update()
    {
        Vector3 camPos = new Vector3(_player.transform.position.x, _player.transform.position.y, transform.position.z);
        transform.position = camPos;
    }
}
