using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class Method
{
    public static float Map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static GameObject GetPlayerObject()
    {
        return GameObject.FindWithTag("Player");
    }

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

    public static void AssignFields(object source, object target)
    {
        Type sourceType = source.GetType();
        Type targetType = target.GetType();

        FieldInfo[] sourceFields = sourceType.GetFields(BindingFlags.Public | BindingFlags.Instance);
        FieldInfo[] targetFields = targetType.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (FieldInfo sourceField in sourceFields)
        {
            foreach (FieldInfo targetField in targetFields)
            {
                if (sourceField.Name == targetField.Name && sourceField.FieldType == targetField.FieldType)
                {
                    targetField.SetValue(target, sourceField.GetValue(source));
                    break;
                }
            }
        }
    }

    public static bool IsMethodSubscribed(Action action, Action method) // #%
    {
        Delegate[] subscribers = action.GetInvocationList();
        foreach (Delegate subscriber in subscribers)
        {
            if (subscriber.Target == method.Target && subscriber.Method == method.Method)
            {
                return true;
            }
        }
        return false;
    }

    public static bool StringMatchesArrayElement(string input, string[] array)
    {
        foreach (string str in array)
        {
            if (str == input)
            {
                return true;
            }
        }
        return false;
    }
}
