using UnityEngine;

[ExecuteInEditMode]
public class PatrolObjectOutline : MonoBehaviour
{
    [Header("Dependencies")] // Required to be assigned in the Inspector
    [SerializeField] Transform _rootParent;

    public enum Side { Top, Bottom, Left, Right }
    [SerializeField] Side _side;

    [Range(0, 1f)]
    [SerializeField] float _stroke = 0.1f;

    void Update()
    {
        Vector2 position = _rootParent.localPosition;
        Vector2 localScale = _rootParent.localScale;

        switch (_side)
        {
            case Side.Top:
                position += _rootParent.localScale.y * Vector2.up / 2;
                localScale = new Vector2(1, 1 / localScale.y * _stroke);
                break;
            case Side.Bottom:
                position += _rootParent.localScale.y * Vector2.down / 2;
                localScale = new Vector2(1, 1 / localScale.y * _stroke);
                break;
            case Side.Left:
                position += _rootParent.localScale.x * Vector2.left / 2;
                localScale = new Vector2(1 / localScale.x * _stroke, 1);
                break;
            case Side.Right:
                position += _rootParent.localScale.x * Vector2.right / 2;
                localScale = new Vector2(1 / localScale.x * _stroke, 1);
                break;
            default:
                break;
        }

        transform.position = new Vector3(position.x, position.y, transform.position.z);
        transform.localScale = new Vector3(localScale.x, localScale.y, transform.localScale.z);
    }
}
