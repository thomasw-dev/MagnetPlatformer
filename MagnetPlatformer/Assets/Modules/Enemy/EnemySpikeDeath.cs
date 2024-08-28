using System.Collections;
using UnityEngine;

public class EnemySpikeDeath : MonoBehaviour
{
    [Header("Dependencies")] // Required to be assigned in the Inspector
    [SerializeField] Transform _rootParent;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.TryGetComponent(out SpikeTiles spikeTiles)) { return; }

        StartCoroutine(WaitSetInactive());
    }

    IEnumerator WaitSetInactive()
    {
        yield return new WaitForSeconds(0.1f);
        _rootParent.gameObject.SetActive(false);
    }
}
