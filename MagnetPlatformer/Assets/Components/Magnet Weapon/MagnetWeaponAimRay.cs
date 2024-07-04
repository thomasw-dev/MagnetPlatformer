using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetWeaponAimRay : MonoBehaviour
{
    LineRenderer _lineRenderer;
    [SerializeField] Transform[] points;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        SetupLines(points);
    }

    void SetupLines(Transform[] points)
    {
        _lineRenderer.positionCount = points.Length;
        this.points = points;
    }

    void Update()
    {
        for (int i = 0; i < points.Length; i++)
        {
            _lineRenderer.SetPosition(i, points[i].position);
        }
    }
}
