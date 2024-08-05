using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionKill : MonoBehaviour
{
    [Header("Dependencies")] // Required to be assigned in the Inspector
    [SerializeField] Transform _rootParent;

    [Space(10)]

    [SerializeField] bool _kill = false;
    [SerializeField] List<GameObject> _collisions;

    const float BOXCAST_DISTANCE = 0.01f;
    const float KILL_SET_INACTIVE_WAIT = 0.1f;
    LayerMask[] _includeLayers;

    void Awake()
    {
        _includeLayers = new LayerMask[]
        {
            LayerMask.GetMask(Constants.LAYER.Environment.ToString()),
            LayerMask.GetMask(Constants.LAYER.Magnetic.ToString())
        };
    }

    void Update()
    {
        _collisions = new List<GameObject>();
        bool isHitTop = false;
        bool isHitBottom = false;
        bool isHitLeft = false;
        bool isHitRight = false;

        foreach (LayerMask layerMask in _includeLayers)
        {
            RaycastHit2D hitTop = Physics2D.BoxCast(transform.position, _rootParent.transform.localScale, 0, Vector2.up, BOXCAST_DISTANCE, layerMask);
            if (hitTop.collider != null)
            {
                _collisions.Add(hitTop.collider.gameObject);
                isHitTop = true;
            }
        }

        foreach (LayerMask layerMask in _includeLayers)
        {
            RaycastHit2D hitBottom = Physics2D.BoxCast(transform.position, _rootParent.transform.localScale, 0, Vector2.down, BOXCAST_DISTANCE, layerMask);
            if (hitBottom.collider != null)
            {
                _collisions.Add(hitBottom.collider.gameObject);
                isHitBottom = true;
            }
        }

        foreach (LayerMask layerMask in _includeLayers)
        {
            RaycastHit2D hitLeft = Physics2D.BoxCast(transform.position, _rootParent.transform.localScale, 0, Vector2.left, BOXCAST_DISTANCE, layerMask);
            if (hitLeft.collider != null)
            {
                _collisions.Add(hitLeft.collider.gameObject);
                isHitLeft = true;
            }
        }

        foreach (LayerMask layerMask in _includeLayers)
        {
            RaycastHit2D hitRight = Physics2D.BoxCast(transform.position, _rootParent.transform.localScale, 0, Vector2.right, BOXCAST_DISTANCE, layerMask);
            if (hitRight.collider != null)
            {
                _collisions.Add(hitRight.collider.gameObject);
                isHitRight = true;
            }
        }

        _kill = (isHitTop && isHitBottom) || (isHitLeft && isHitRight);

        if (_kill)
        {
            StartCoroutine(WaitSetInactive());
        }
    }

    IEnumerator WaitSetInactive()
    {
        yield return new WaitForSeconds(KILL_SET_INACTIVE_WAIT);
        _rootParent.gameObject.SetActive(false);
    }
}
