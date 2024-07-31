using UnityEngine;

public class MagnetGunFireRay : MonoBehaviour
{
    [Header("Dependencies")]
    // These fields are required to be assigned in order for this module to function.

    public MagnetGunController _magnetGunController;

    [SerializeField] Transform[] points;
    [SerializeField] Material[] _aimRayMaterials;

    const float LINE_WIDTH_WIDE = 1f;
    const float LINE_WIDTH_THIN = 0.1f;

    LineRenderer _lineRenderer;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void OnEnable()
    {
        GameState.Initialize.OnEnter += Initialize;
        InputManager.OnMagnetGunSetCharge += ChangeLineColorByCharge;
        _magnetGunController.OnFire += Fire;
        _magnetGunController.OnFireRelease += FireRelease;
        GameState.Play.OnExit += Disable;
    }

    void OnDisable()
    {
        GameState.Initialize.OnEnter -= Initialize;
        InputManager.OnMagnetGunSetCharge -= ChangeLineColorByCharge;
        _magnetGunController.OnFire -= Fire;
        _magnetGunController.OnFireRelease -= FireRelease;
        GameState.Play.OnExit -= Disable;
    }

    void Start()
    {
        SetupLines(points);
    }

    void Initialize()
    {
        ChangeLineColorByCharge(Magnet.Charge.Neutral);
    }

    void Update()
    {
        for (int i = 0; i < points.Length; i++)
        {
            _lineRenderer.SetPosition(i, points[i].position);
        }
    }

    void SetupLines(Transform[] points)
    {
        _lineRenderer.positionCount = points.Length;
        this.points = points;
    }

    void ChangeLineColorByCharge(Magnet.Charge charge)
    {
        if (charge == Magnet.Charge.Neutral) _lineRenderer.material = _aimRayMaterials[0];
        if (charge == Magnet.Charge.Positive) _lineRenderer.material = _aimRayMaterials[1];
        if (charge == Magnet.Charge.Negative) _lineRenderer.material = _aimRayMaterials[2];
    }

    void Fire(Magnet.Charge charge)
    {
        _lineRenderer.startWidth = LINE_WIDTH_WIDE;
        _lineRenderer.endWidth = LINE_WIDTH_WIDE;
    }

    void FireRelease()
    {
        _lineRenderer.startWidth = LINE_WIDTH_THIN;
        _lineRenderer.endWidth = LINE_WIDTH_THIN;
    }

    void Disable()
    {
        ChangeLineColorByCharge(Magnet.Charge.Neutral);
        _lineRenderer.enabled = false;
    }
}
