using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetWeaponAimRay : MonoBehaviour
{
    LineRenderer _lineRenderer;
    [SerializeField] Transform[] points;

    [SerializeField] Material[] _aimRayMaterials;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void OnEnable()
    {
        GameManager.OnInitializationEnter += Initialize;
        InputManager.OnMagnetWeaponSetCharge += SetLineVisual;
        GameManager.OnPlayingExit += Restore;
    }

    void OnDisable()
    {
        GameManager.OnInitializationEnter -= Initialize;
        InputManager.OnMagnetWeaponSetCharge -= SetLineVisual;
        GameManager.OnPlayingExit -= Restore;
    }

    void Start()
    {
        SetupLines(points);
    }

    void Initialize()
    {
        SetLineVisual(Magnet.Charge.Neutral);
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

    void SetLineVisual(Magnet.Charge charge)
    {
        if (charge == Magnet.Charge.Neutral)
        {
            _lineRenderer.enabled = false;
        }
        else
        {
            _lineRenderer.enabled = true;
            _lineRenderer.material = GetAimRayMaterialByCharge(charge);
            Material GetAimRayMaterialByCharge(Magnet.Charge charge) => charge switch
            {
                Magnet.Charge.Positive => _aimRayMaterials[0],
                Magnet.Charge.Negative => _aimRayMaterials[1],
                _ => _aimRayMaterials[0]
            };
        }
    }

    void Restore() => SetLineVisual(Magnet.Charge.Neutral);
}
