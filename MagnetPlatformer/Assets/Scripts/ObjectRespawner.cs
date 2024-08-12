using UnityEngine;

public class ObjectRespawner : MonoBehaviour
{
    [SerializeField] GameObject _object;

    [SerializeField] bool _objectInArea = true;

    [SerializeField] float _respawnAfter = 1f;
    float _leaveAreaTime;

    void Update()
    {
        if (!_objectInArea)
        {
            if (Time.time > _leaveAreaTime + _respawnAfter)
            {
                // Spawn the object
                GameObject respanwedObject = Instantiate(_object, transform.position, Quaternion.identity);
                _objectInArea = true;

                // Start the spawned object's self-destruct countdown
                if (respanwedObject.TryGetComponent(out SelfDestructTimer selfDestructTimer) && selfDestructTimer.isActiveAndEnabled)
                {
                    selfDestructTimer.StartCountdown();
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject != _object) { return; }

        _objectInArea = false;
        _leaveAreaTime = Time.time;
    }

    [ContextMenu("Update To Target Object Position & Scale")]
    void UpdateToTargetObjectPositionScale()
    {
        if (_object == null) { return; }

        transform.position = _object.transform.position;
        transform.localScale = _object.transform.localScale;
    }
}
