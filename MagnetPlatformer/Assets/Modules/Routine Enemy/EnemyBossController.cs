using UnityEngine;

public class EnemyBossController : MonoBehaviour
{
    // State

    public enum StateEnum { Idle, Active }
    public StateController<StateEnum> StateController = new StateController<StateEnum>();
    [SerializeField] StateEnum _state; // Inspector

    [Tooltip("If unassigned, it will be activated on Start().")]
    [Header("Activation")]
    [SerializeField] EnemyActivateTrigger _activateTrigger;

    void OnEnable()
    {
        if (_activateTrigger != null)
        {
            _activateTrigger.OnActivate += Activate;
        }
    }

    void OnDisable()
    {
        if (_activateTrigger != null)
        {
            _activateTrigger.OnActivate -= Activate;
        }
    }

    void Start()
    {
        StateController.ChangeState(_activateTrigger == null ? StateEnum.Idle : StateEnum.Active);
    }

    void Update()
    {
        _state = StateController.CurrentEnum; // Inspector
    }

    void Activate()
    {
        StateController.ChangeState(StateEnum.Active);
    }
}