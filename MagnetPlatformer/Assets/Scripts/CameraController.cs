using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject _player;

    [Range(1f, 10f)]
    [SerializeField] float _smoothFactor = 1f;

    void Update()
    {
        Vector3 camPos = new Vector3(_player.transform.position.x, _player.transform.position.y, transform.position.z);
        //transform.position = camPos;
        transform.position = Vector3.Lerp(transform.position, camPos, _smoothFactor * Time.deltaTime);

    }
}
