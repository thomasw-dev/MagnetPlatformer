using UnityEngine;

[ExecuteInEditMode]
public class TiledSpriteScaler : MonoBehaviour
{
    [SerializeField] Transform parent;

    void Update()
    {
        if (parent == null) { return; }

        transform.localScale = new Vector3(1 / parent.localScale.x, 1 / parent.localScale.y, 1 / parent.localScale.z);

        if (GetComponent<SpriteRenderer>() == null) { return; }

        GetComponent<SpriteRenderer>().size = new Vector2(parent.localScale.x, parent.localScale.y);
    }
}