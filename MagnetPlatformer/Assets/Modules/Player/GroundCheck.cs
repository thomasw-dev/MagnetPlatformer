using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool IsGrounded { get => _isGrounded; }
    [SerializeField] bool _isGrounded;

    [Space(10)]

    const float GROUND_CHECK_BOXCAST_HEIGHT = 0.1f;
    const float GROUND_CHECK_BOXCAST_LENGTH = 0.3f;

    LayerMask[] _groundLayers;

    void Awake()
    {
        _groundLayers = new LayerMask[]
        {
            LayerMask.GetMask(Constants.LAYER.Physics.ToString()),
            LayerMask.GetMask(Constants.LAYER.Environment.ToString())
        };
    }

    void OnTriggerStay2D(Collider2D col)
    {
        string layerName = col.gameObject.layer.ToString();
        if (layerName == Constants.LAYER.Physics.ToString() ||
            layerName == Constants.LAYER.Environment.ToString())
        {
            _isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
