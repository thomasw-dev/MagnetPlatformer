using System.Collections.Generic;
using UnityEngine;

public class ObjectRespawner : MonoBehaviour
{
    [SerializeField] GameObject _object;
    [SerializeField] bool _objectInArea = false;
    [SerializeField] float _respawnAfter = 1f;
    [SerializeField] List<GameObject> spawnedObjects;

    [SerializeField] GameObject _lastSpawnedObject;
    [SerializeField] float _lastInAreaTime = 0f;

    void Start()
    {
        SpawnObject(_object);
    }

    void Update()
    {
        if (!_objectInArea)
        {
            if (Time.time > _lastInAreaTime + _respawnAfter)
                SpawnObject(_object);
        }
        else
        {
            _lastInAreaTime = Time.time;
        }
    }

    void SpawnObject(GameObject obj)
    {
        if (obj == null) { return; }

        // Spawn the object
        GameObject spawnedObject = Instantiate(_object, transform.position, Quaternion.identity);
        StartObjectSelfDestructCountdown(spawnedObject);
        spawnedObjects.Add(spawnedObject);
        _lastSpawnedObject = spawnedObject;
        _objectInArea = true;
    }

    void StartObjectSelfDestructCountdown(GameObject obj)
    {
        if (obj == null) { return; }

        // Start the spawned object's self-destruct countdown
        if (obj.TryGetComponent(out SelfDestructTimer selfDestructTimer) && selfDestructTimer.isActiveAndEnabled)
        {
            selfDestructTimer.StartCountdown();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        Debug.Log(collider.gameObject.name);

        if (collider.transform.parent.gameObject != _lastSpawnedObject) { return; }

        _objectInArea = false;
    }

    [ContextMenu("Update To Target Object Position & Scale")]
    void UpdateToTargetObjectPositionScale()
    {
        if (_object == null) { return; }

        transform.position = _object.transform.position;
        transform.localScale = _object.transform.localScale;
    }
}
