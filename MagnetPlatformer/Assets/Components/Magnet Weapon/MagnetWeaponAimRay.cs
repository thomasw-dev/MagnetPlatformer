using UnityEngine;

public class MagnetWeaponAimRay : MonoBehaviour
{
    LineRenderer _lineRenderer;
    [SerializeField] Transform[] points;
    [SerializeField] Material[] _aimRayMaterials;
    LayerMask _excludeLayers;

    const float LINE_WIDTH_WIDE = 1f;
    const float LINE_WIDTH_THIN = 0.1f;
    const float RAYCAST_LENGTH = 100f;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void OnEnable()
    {
        GameState.Initialize.OnEnter += Initialize;
        InputManager.OnMagnetWeaponSetCharge += SetLineVisual;
        MagnetWeapon.OnFireWeapon += ShootRay;
        MagnetWeapon.OnFireWeaponStop += ShootRayStop;
        GameState.Play.OnExit += Disable;
    }

    void OnDisable()
    {
        GameState.Initialize.OnEnter -= Initialize;
        InputManager.OnMagnetWeaponSetCharge -= SetLineVisual;
        MagnetWeapon.OnFireWeapon -= ShootRay;
        MagnetWeapon.OnFireWeaponStop -= ShootRayStop;
        GameState.Play.OnExit -= Disable;
    }

    void Start()
    {
        _excludeLayers = LayerMask.GetMask(
            Constants.LAYER.Physics.ToString(),
            Constants.LAYER.Player.ToString(),
            Constants.LAYER.Environment.ToString()
        );
        SetupLines(points);
    }

    void Initialize()
    {
        SetLineVisual(Magnet.Charge.Neutral);
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

    void SetLineVisual(Magnet.Charge charge)
    {
        float width = GetAimRayWidthByCharge(charge);
        float GetAimRayWidthByCharge(Magnet.Charge charge) => charge switch
        {
            Magnet.Charge.Neutral => LINE_WIDTH_THIN,
            Magnet.Charge.Positive => LINE_WIDTH_THIN,
            Magnet.Charge.Negative => LINE_WIDTH_THIN,
            _ => 1f
        };
        _lineRenderer.startWidth = width;
        _lineRenderer.endWidth = width;

        _lineRenderer.material = GetAimRayMaterialByCharge(charge);
        Material GetAimRayMaterialByCharge(Magnet.Charge charge) => charge switch
        {
            Magnet.Charge.Neutral => _aimRayMaterials[0],
            Magnet.Charge.Positive => _aimRayMaterials[1],
            Magnet.Charge.Negative => _aimRayMaterials[2],
            _ => _aimRayMaterials[0]
        };
    }

    void ShootRay(Magnet.Charge charge)
    {
        _lineRenderer.startWidth = LINE_WIDTH_WIDE;
        _lineRenderer.endWidth = LINE_WIDTH_WIDE;

        Vector2 origin = transform.position;
        Vector2 direction = transform.up;
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, RAYCAST_LENGTH, ~_excludeLayers);

        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;

            if (Log.MagnetWeaponHit)
            {
                Debug.Log("Hit: " + hitObject.name);
            }

            if (hitObject.TryGetComponent(out MagneticObjectController magneticObject))
            {
                magneticObject.AlterCharge(charge);
            }
        }
    }

    void ShootRayStop()
    {
        _lineRenderer.startWidth = LINE_WIDTH_THIN;
        _lineRenderer.endWidth = LINE_WIDTH_THIN;
    }

    void Disable()
    {
        SetLineVisual(Magnet.Charge.Neutral);
        _lineRenderer.enabled = false;
    }
}
