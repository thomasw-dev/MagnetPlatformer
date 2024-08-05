using System.Collections.Generic;
using UnityEngine;

public class CollisionList : MonoBehaviour
{
    [SerializeField] List<GameObject> collisions = new List<GameObject>();

    void OnCollisionEnter2D(Collision2D col)
    {
        collisions.Add(col.gameObject);
    }

    void OnCollisionExit2D(Collision2D col)
    {
        collisions.Remove(col.gameObject);
    }
}