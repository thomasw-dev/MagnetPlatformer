using UnityEngine;

[ExecuteInEditMode]
public class AlterChargeTiledSpriteScaler : MonoBehaviour
{
    [SerializeField] Transform parent;

    void Update()
    {
        if (parent == null) { return; }

        float x = parent.localScale.x > 0 ? 1 / parent.localScale.x : 0;
        float y = parent.localScale.y > 0 ? 1 / parent.localScale.y : 0;
        float z = parent.localScale.z > 0 ? 1 / parent.localScale.z : 0;
        //transform.localScale = new Vector3(x, y, z);
        transform.localScale = new Vector3(1, 1, 1);

        if (GetComponent<SpriteRenderer>() == null) { return; }

        //GetComponent<SpriteRenderer>().size = new Vector2(parent.localScale.x, parent.localScale.y);
        GetComponent<SpriteRenderer>().size = new Vector2(1, 1);
    }
}