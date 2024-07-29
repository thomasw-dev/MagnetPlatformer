using UnityEngine;

public class BoxColliderSetter : MonoBehaviour
{
    public enum PivotPoint { Center, TopLeft, TopRight, BottomLeft, BottomRight }
    PivotPoint CurrentPivot;

    public PivotPoint Pivot; // Inspector
    public Vector2 Size = Vector2.one;

    BoxCollider2D GetCollider() => GetComponent<BoxCollider2D>();

    void OnValidate()
    {
        void OffsetTransform(float x, float y)
        {
            transform.localPosition += transform.localScale.x * Vector3.right * x;
            transform.localPosition += transform.localScale.y * Vector3.up * y;
        }

        if (Pivot != CurrentPivot)
        {
            if (CurrentPivot == PivotPoint.Center)
            {
                if (Pivot == PivotPoint.TopLeft)
                    OffsetTransform(-Size.x / 2, Size.y / 2);

                if (Pivot == PivotPoint.TopRight)
                    OffsetTransform(Size.x / 2, Size.y / 2);

                if (Pivot == PivotPoint.BottomLeft)
                    OffsetTransform(-Size.x / 2, -Size.y / 2);

                if (Pivot == PivotPoint.BottomRight)
                    OffsetTransform(Size.x / 2, -Size.y / 2);
            }

            if (CurrentPivot == PivotPoint.TopLeft)
            {
                if (Pivot == PivotPoint.Center)
                    OffsetTransform(Size.x / 2, -Size.y / 2);

                if (Pivot == PivotPoint.TopRight)
                    OffsetTransform(Size.x, 0);

                if (Pivot == PivotPoint.BottomLeft)
                    OffsetTransform(0, -Size.y);

                if (Pivot == PivotPoint.BottomRight)
                    OffsetTransform(Size.x, -Size.y);
            }

            if (CurrentPivot == PivotPoint.TopRight)
            {
                if (Pivot == PivotPoint.Center)
                    OffsetTransform(-Size.x / 2, -Size.y / 2);

                if (Pivot == PivotPoint.TopLeft)
                    OffsetTransform(-Size.x, 0);

                if (Pivot == PivotPoint.BottomLeft)
                    OffsetTransform(-Size.x, -Size.y);

                if (Pivot == PivotPoint.BottomRight)
                    OffsetTransform(0, -Size.y);
            }

            if (CurrentPivot == PivotPoint.BottomLeft)
            {
                if (Pivot == PivotPoint.Center)
                    OffsetTransform(Size.x / 2, Size.y / 2);

                if (Pivot == PivotPoint.TopLeft)
                    OffsetTransform(0, Size.y);

                if (Pivot == PivotPoint.TopRight)
                    OffsetTransform(Size.x, Size.y);

                if (Pivot == PivotPoint.BottomRight)
                    OffsetTransform(Size.x, 0);
            }

            if (CurrentPivot == PivotPoint.BottomRight)
            {
                if (Pivot == PivotPoint.Center)
                    OffsetTransform(-Size.x / 2, Size.y / 2);

                if (Pivot == PivotPoint.TopLeft)
                    OffsetTransform(-Size.x, Size.y);

                if (Pivot == PivotPoint.TopRight)
                    OffsetTransform(0, Size.y);

                if (Pivot == PivotPoint.BottomLeft)
                    OffsetTransform(-Size.x, 0);
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