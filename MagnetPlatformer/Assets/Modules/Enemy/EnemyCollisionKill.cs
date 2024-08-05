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

    const float BOXCAST_DISTANCE = 0.01f;

    void Update()
    {
        _includedCollisions = GetIncludedCollisions(_rigidbodyCollision.List);
        //Debug.Log(_includedCollisions.Count);

        // Perform a box cast towards the top side
        RaycastHit2D hitTop = Physics2D.BoxCast(transform.position, _rigidbodyCollision.transform.localScale, 0, Vector2.up, BOXCAST_DISTANCE);

        if (hitTop.collider != null)
        {
            Debug.Log("Top side hit: " + hitTop.collider.name);
        }

        // Perform a box cast towards the bottom side
        RaycastHit2D hitBottom = Physics2D.BoxCast(transform.position, _rigidbodyCollision.transform.localScale, 0, Vector2.down, BOXCAST_DISTANCE);

        if (hitBottom.collider != null)
        {
            Debug.Log("Bottom side hit: " + hitBottom.collider.name);
        }

        // Check if both sides are hit
        if (hitTop.collider != null && hitBottom.collider != null)
        {
            Debug.Log("Both top and bottom sides are collided!");
        }
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
