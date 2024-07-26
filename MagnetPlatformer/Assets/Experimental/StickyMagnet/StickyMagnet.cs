using UnityEngine;

public class StickyMagnet : MonoBehaviour
{
    [SerializeField] Rigidbody2D _target;
    [SerializeField] float _radius = 7;
    [SerializeField] float _factor = 1;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector2 selfPos = transform.position;
        Vector2 targetPos = _target.transform.position;
        Vector2 distance = selfPos - targetPos;

        float dX = Method.Map(distance.x, 0, _radius, _radius, 0);
        float dY = Method.Map(distance.y, 0, _radius, _radius, 0);
        Vector2 closeness = new Vector2(dX, dY);

        if (Vector3.Distance(selfPos, targetPos) <= _radius)
        {
            _target.GetComponent<RigidbodyMoveKeys>().enabled = false;
            _target.MovePosition((Vector2)_target.transform.position + distance * Time.deltaTime * _factor);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
