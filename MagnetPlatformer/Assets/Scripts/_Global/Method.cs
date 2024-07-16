using System;
using System.Collections.Generic;
using UnityEngine;

public static class Method
{
    public static bool IsPlayerObject(GameObject gameObject)
    {
        return gameObject.tag == "Player" ? true : false;
    }

    public static List<GameObject> GetChildrenMeetCondition(GameObject user, Func<GameObject, bool> condition)
    {
        List<GameObject> output = new List<GameObject>();
        List<GameObject> children = new List<GameObject>();
        GetChildrenRecursive(user, ref children);
        foreach (GameObject child in children)
        {
            if (condition(child))
            { output.Add(child); }
        }
        return output;
    }

    public static void GetChildrenRecursive(GameObject obj, ref List<GameObject> children)
    {
        foreach (Transform child in obj.transform)
        {
            if (!child) continue;
            else children.Add(child.gameObject);
            GetChildrenRecursive(child.gameObject, ref children);
        }
    }

    public static bool IsInLayer(GameObject gameObject, string layerName)
    {
        int layerMask = LayerMask.GetMask(layerName);
        return (layerMask & (1 << gameObject.layer)) != 0;
    }
}
