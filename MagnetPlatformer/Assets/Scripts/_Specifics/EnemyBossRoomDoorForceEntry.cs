using DG.Tweening;
using UnityEngine;

public class EnemyBossRoomDoorForceEntry : MonoBehaviour
{
    // State

    public enum StateEnum { Before, Entered }
    public StateController<StateEnum> StateController = new StateController<StateEnum>();
    [SerializeField] StateEnum _state; // Inspector

    [Header("Assign Fields")]
    [SerializeField] GameObject _hint;
    [SerializeField] GameObject _invisibleMagneticWall;
    [SerializeField] GameObject _enterTrigger;
    [SerializeField] GameObject _visualWall;
    [SerializeField] GameObject _physicsCollider;

    [SerializeField] float _pushSpeed = 1f;

    void OnEnable()
    {
        StateController.EnumToState(StateEnum.Before).OnEnter += HandleEnterBeforeState;
        StateController.EnumToState(StateEnum.Entered).OnEnter += HandleEnterEnteredState;
        if (_enterTrigger != null)
        {
            _enterTrigger.GetComponent<EnemyBossRoomDoorForceEntryEnterTrigger>().OnPlayerExitRight += EnterEnteredState;
        }
    }

    void OnDisable()
    {
        StateController.EnumToState(StateEnum.Before).OnEnter -= HandleEnterBeforeState;
        StateController.EnumToState(StateEnum.Entered).OnEnter -= HandleEnterEnteredState;
        if (_enterTrigger != null)
        {
            _enterTrigger.GetComponent<EnemyBossRoomDoorForceEntryEnterTrigger>().OnPlayerExitRight -= EnterEnteredState;
        }
    }

    void Start()
    {
        StateController.ChangeState(StateEnum.Before);
    }

    void EnterEnteredState()
    {
        StateController.ChangeState(StateEnum.Entered);
    }

    void HandleEnterBeforeState()
    {
        _hint.SetActive(true);
        _invisibleMagneticWall.SetActive(true);
        _visualWall.SetActive(false);
        _physicsCollider.SetActive(false);
    }

    void HandleEnterEnteredState()
    {
        _hint.SetActive(false);
        _invisibleMagneticWall.SetActive(false);
        _visualWall.SetActive(true);
        _physicsCollider.SetActive(true);

        Vector3 initialPos = transform.position;
        transform.position += Vector3.left * 2;
        transform.DOMove(initialPos, _pushSpeed).SetEase(Ease.Linear);
    }
}
