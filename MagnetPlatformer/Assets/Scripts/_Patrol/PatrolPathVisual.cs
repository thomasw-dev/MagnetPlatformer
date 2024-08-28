using UnityEngine;

[ExecuteInEditMode]
public class PatrolPathVisual : MonoBehaviour
{
    [Range(0, 1f)]
    [SerializeField] float _lineWidth = 0.1f;
    [SerializeField] Transform[] points;

    LineRenderer _lineRenderer;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
        SetupLines(points);
    }

    void Update()
    {
        for (int i = 0; i < points.Length; i++)
        {
            _lineRenderer.SetPosition(i, points[i].position);
        }

        _lineRenderer.startWidth = _lineWidth;
        _lineRenderer.endWidth = _lineWidth;
    }

    void SetupLines(Transform[] points)
    {
        _lineRenderer.positionCount = points.Length;
    }
}
