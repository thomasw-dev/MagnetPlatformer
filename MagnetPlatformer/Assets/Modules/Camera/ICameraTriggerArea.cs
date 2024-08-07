using UnityEngine;

public abstract class ICameraTriggerArea : MonoBehaviour
{
    [Range(0, 10f)]
    [SerializeField] float _transitionTime = 1f;
    
    public abstract void OnTriggerEnter2D();
    public abstract void OnTriggerExit2D();
}
