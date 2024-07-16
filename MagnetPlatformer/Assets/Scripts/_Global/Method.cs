using System;
using System.Collections.Generic;
using UnityEngine;

public static class Method
{
    public static bool IsPlayerObject(GameObject gameObject)
    {
        return gameObject.tag == "Player" ? true : false;
    }

    public static bool IsInLayer(GameObject gameObject, string layerName)
    {
        int layerMask = LayerMask.GetMask(layerName);
        return (layerMask & (1 << gameObject.layer)) != 0;
    }

    public static bool HasEnabledComponent<T>(GameObject gameObject) where T : Component
    {
        if (gameObject.TryGetComponent(out T component))
        {
            if (component is Behaviour behaviour)
            {
                return behaviour.enabled;
            }
            else return true;
        }
        else return false;
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

    public static void GetChildrenRecursive(GameObject user, ref List<GameObject> children)
    {
        foreach (Transform child in user.transform)
        {
            if (!child) continue;
            else children.Add(child.gameObject);
            GetChildrenRecursive(child.gameObject, ref children);
        }
    }
}
