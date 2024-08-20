using UnityEngine;

[ExecuteInEditMode]
public class ColorOverlayTiledSpriteScaler : MonoBehaviour
{
    [SerializeField] Transform parent;

    void Update()
    {
        if (parent == null) { return; }

        float x = parent.localScale.x > 0 ? 1 / parent.localScale.x : 0;
        transform.localScale = new Vector3(x, x, x);

        if (GetComponent<SpriteRenderer>() == null) { return; }

        GetComponent<SpriteRenderer>().size = new Vector2(parent.localScale.x, parent.localScale.y);
    }
}