using DG.Tweening;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class EnemyCollisionKill : MonoBehaviour
{
    [Header("Dependencies")] // Required to be assigned in the Inspector
    [SerializeField] Transform _rootParent;

    //const float KILL_SET_INACTIVE_WAIT = 0.1f;

    [SerializeField] float _squeezeDuration = 1f;

    CollisionKill _collisionKill;

    void Awake()
    {
        _collisionKill = GetComponent<CollisionKill>();
    }

    void OnEnable()
    {
        _collisionKill.OnKill += HandleKill;
        _collisionKill.OnDirectionKill += HandleDirectionKill;
    }

    void OnDisable()
    {
        _collisionKill.OnKill -= HandleKill;
        _collisionKill.OnDirectionKill -= HandleDirectionKill;
    }

    void HandleKill()
    {
        //StartCoroutine(WaitSetInactive());
    }

    /*IEnumerator WaitSetInactive()
    {
        yield return new WaitForSeconds(KILL_SET_INACTIVE_WAIT);
        _rootParent.gameObject.SetActive(false);
    }*/

    void HandleDirectionKill(Direction.Type direction)
    {
        Vector3 toScale = Vector3.one;
        if (direction == Direction.Type.Vertical) toScale = new Vector3(_rootParent.localScale.x, 0, _rootParent.localScale.z);
        if (direction == Direction.Type.Horizontal) toScale = new Vector3(0, _rootParent.localScale.y, _rootParent.localScale.z);

        _rootParent.DOScale(toScale, _squeezeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _rootParent.gameObject.SetActive(false);
            });
    }
}
