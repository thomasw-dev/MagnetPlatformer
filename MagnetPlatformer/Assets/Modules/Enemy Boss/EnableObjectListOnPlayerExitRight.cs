using UnityEngine;

public class EnableObjectListOnPlayerExitRight : MonoBehaviour
{
    [SerializeField] bool _enabled;
    [SerializeField] GameObject[] _list;

    void Start()
    {
        foreach (var item in _list)
        {
            item.SetActive(false);
        }
    }

    void Update()
    {
        if (_enabled)
        {
            foreach (var item in _list)
            {
                item.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        GameObject player = collider.gameObject;

        if (!Method.IsPlayerObject(player)) { return; }

        if (player.transform.position.x > transform.position.x)
        { _enabled = true; }
    }
}
