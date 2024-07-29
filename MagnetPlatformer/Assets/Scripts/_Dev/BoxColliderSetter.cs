using UnityEngine;

public class BoxColliderSetter : MonoBehaviour
{
    public enum PivotPoint { Center, TopLeft, TopRight, BottomLeft, BottomRight }
    PivotPoint CurrentPivot;

    public PivotPoint Pivot; // Inspector
    public Vector2 Size;

    BoxCollider2D GetCollider() => GetComponent<BoxCollider2D>();

    void OnValidate()
    {
        if (Pivot != CurrentPivot)
        {
            // Center to...

            if (CurrentPivot == PivotPoint.Center && Pivot == PivotPoint.TopLeft)
            {
                transform.localPosition -= Vector3.right * (Size.x / 2);
                transform.localPosition += Vector3.up * (Size.y / 2);
            }

            if (CurrentPivot == PivotPoint.Center && Pivot == PivotPoint.TopRight)
            {
                transform.localPosition += Vector3.right * (Size.x / 2);
                transform.localPosition += Vector3.up * (Size.y / 2);
            }

            if (CurrentPivot == PivotPoint.Center && Pivot == PivotPoint.BottomLeft)
            {
                transform.localPosition -= Vector3.right * (Size.x / 2);
                transform.localPosition -= Vector3.up * (Size.y / 2);
            }

            if (CurrentPivot == PivotPoint.Center && Pivot == PivotPoint.BottomRight)
            {
                transform.localPosition += Vector3.right * (Size.x / 2);
                transform.localPosition -= Vector3.up * (Size.y / 2);
            }

            // Top Left to...

            if (CurrentPivot == PivotPoint.TopLeft && Pivot == PivotPoint.Center)
            {
                transform.localPosition += Vector3.right * (Size.x / 2);
                transform.localPosition -= Vector3.up * (Size.y / 2);
            }

            // Top Right to...

            if (CurrentPivot == PivotPoint.TopRight && Pivot == PivotPoint.Center)
            {
                transform.localPosition -= Vector3.right * (Size.x / 2);
                transform.localPosition -= Vector3.up * (Size.y / 2);
            }

            // Bottom Left to...

            if (CurrentPivot == PivotPoint.BottomLeft && Pivot == PivotPoint.Center)
            {
                transform.localPosition += Vector3.right * (Size.x / 2);
                transform.localPosition += Vector3.up * (Size.y / 2);
            }

            // Bottom Right to...

            if (CurrentPivot == PivotPoint.BottomRight && Pivot == PivotPoint.Center)
            {
                transform.localPosition -= Vector3.right * (Size.x / 2);
                transform.localPosition += Vector3.up * (Size.y / 2);
            }
        }

        CurrentPivot = Pivot;

        switch (CurrentPivot)
        {
            case PivotPoint.Center:
                GetCollider().offset = Vector2.zero;
                break;
            case PivotPoint.TopLeft:
                GetCollider().offset = new Vector2(Size.x / 2, -Size.y / 2);
                break;
            case PivotPoint.TopRight:
                GetCollider().offset = new Vector2(-Size.x / 2, -Size.y / 2);
                break;
            case PivotPoint.BottomLeft:
                GetCollider().offset = new Vector2(Size.x / 2, Size.y / 2);
                break;
            case PivotPoint.BottomRight:
                GetCollider().offset = new Vector2(-Size.x / 2, Size.y / 2);
                break;
           default:
                break;
        }

        GetCollider().size = Size;
    }
}