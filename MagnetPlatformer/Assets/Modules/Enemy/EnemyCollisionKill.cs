using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionKill : MonoBehaviour
{
    [Header("Dependencies")] // Required to be assigned in the Inspector

    [SerializeField] RigidbodyCollision _rigidbodyCollision;

    List<GameObject> _includedCollisions;
    string[] _includeTags = {
        Constants.TAG[Constants.ENUM_TAG.ENVIRONMENT],
        Constants.TAG[Constants.ENUM_TAG.MAGNETIC_OBJECT],
        Constants.TAG[Constants.ENUM_TAG.ENEMY],
        Constants.TAG[Constants.ENUM_TAG.PROP_OBJECT]
    };

    void Update()
    {
        _includedCollisions = GetIncludedCollisions(_rigidbodyCollision.List);
        Debug.Log(_includedCollisions.Count);
    }

    List<GameObject> GetIncludedCollisions(List<GameObject> list)
    {
        List<GameObject> output = new List<GameObject>();

        foreach (var item in list)
        {
            if (StringMatchesArrayElement(item.tag, _includeTags))
            {
                output.Add(item);
            }
        }

        return output;
    }

    bool StringMatchesArrayElement(string input, string[] array)
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
