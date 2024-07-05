using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticObjectVisual : MonoBehaviour
{
    [SerializeField] MagneticObject _magneticObject;

    void Awake()
    {
        NullCheck();
    }

    void NullCheck()
    {
        if (_magneticObject == null)
        {
            Debug.LogError("MagneticObjectVisual is not assigned a MagneticObject.", this);
        }
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
