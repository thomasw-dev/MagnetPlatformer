using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Magnet Object Group", order = 1)]
public class MagnetObjectGroup : RuntimeSet<GameObject>
{
    private List<GameObject> _items = new();

    public List<GameObject> Items
    {
        get { return _items; }
        set { _items = value; }
    }

    public override void Add(GameObject gameObject)
    {
        if (!Items.Contains(gameObject))
        {
            Items.Add(gameObject);
        }
    }

    public override void Remove(GameObject gameObject)
    {
        if (Items.Contains(gameObject))
        {
            Items.Remove(gameObject);
        }
    }
}