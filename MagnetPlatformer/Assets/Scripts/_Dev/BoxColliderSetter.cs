using UnityEngine;

public class BoxColliderSetter : MonoBehaviour
{
    public enum PivotPoint { Center, TopLeft, BottomLeft, TopRight, BottomRight }
    public PivotPoint Pivot;
    public Vector2 Size;

    //Vector2 offsetValue;
    //Vector2 sizeValue;

    BoxCollider2D GetCollider() => GetComponent<BoxCollider2D>();

    void OnValidate()
    {
        switch (Pivot)
        {
            case PivotPoint.Center:
                break;
            case PivotPoint.TopLeft:
                SetOffset();
                SetSize();
                break;
            case PivotPoint.BottomLeft:
                break;
            case PivotPoint.TopRight:
                break;
            case PivotPoint.BottomRight:
                break;
           default:
                break;
        }
    }

    void SetOffset()
    {
        GetCollider().offset = Vector2.zero;
    }

    void SetSize()
    {
        GetCollider().size = Vector2.zero;
    }
}