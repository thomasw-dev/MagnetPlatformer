using UnityEngine;

public class SelfDestructTimer : MonoBehaviour
{
    [SerializeField] float _destroyAfter = 10f;
    [SerializeField] bool _inCountdown = false;
    float _startCountdownTime;

    [ContextMenu("Start Countdown")]
    public void StartCountdown()
    {
        _inCountdown = true;
        _startCountdownTime = Time.time;
    }

    void Update()
    {
        if (_inCountdown && Time.time > _startCountdownTime + _destroyAfter)
        {
            Destroy(gameObject);
        }
    }
}
