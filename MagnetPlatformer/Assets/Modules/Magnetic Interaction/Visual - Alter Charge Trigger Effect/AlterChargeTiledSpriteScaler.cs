using UnityEngine;

[ExecuteInEditMode]
public class AlterChargeTiledSpriteScaler : MonoBehaviour
{
    [SerializeField] Transform parent;

    void Update()
    {
        if (parent == null) { return; }
        if (GetComponent<SpriteRenderer>() == null) { return; }

        GetComponent<SpriteRenderer>().size = new Vector2(parent.localScale.x, parent.localScale.y);
    }
}