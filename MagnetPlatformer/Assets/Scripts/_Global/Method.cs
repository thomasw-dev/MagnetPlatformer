using System.Collections.Generic;
using UnityEngine;

public static class Method
{
    public static bool IsPlayerObject(GameObject gameObject)
    {
        return gameObject.tag == "Player" ? true : false;
    }

    public static void GetChildRecursive_MatchesLayer(GameObject obj, List<GameObject> listOfChildren, LayerMask layer)
    {
        if (obj == null) return;

        foreach (Transform child in obj.transform)
        {
            if (child == null) continue;
            else if (child.gameObject.layer == layer)
                listOfChildren.Add(child.gameObject);

            GetChildRecursive_MatchesLayer(child.gameObject, listOfChildren, layer);
        }
    }

    public static void GetChildRecursive_ContainsSpriteRenderer(GameObject obj, List<GameObject> listOfChildren)
    {
        if (obj == null) return;

        foreach (Transform child in obj.transform)
        {
            if (child == null) continue;
            else if (child.gameObject.TryGetComponent(out SpriteRenderer spriteRenderer))
                listOfChildren.Add(child.gameObject);

            GetChildRecursive_ContainsSpriteRenderer(child.gameObject, listOfChildren);
        }
    }
}
