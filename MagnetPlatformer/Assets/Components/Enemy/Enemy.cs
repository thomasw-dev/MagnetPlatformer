using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnumState
    {
        Idle, Chase
    }
    public StateController<EnumState> StateController = new StateController<EnumState>();
    [SerializeField] EnumState _state; // Inspector
    [SerializeField] EnemyActiveArea _activeArea;

    [HideInInspector] public Vector2 MoveTargetPos;

    void OnEnable()
    {
        _activeArea.OnPlayerEnter += Chase;
        _activeArea.OnPlayerExit += Idle;
    }

    void OnDisable()
    {
        _activeArea.OnPlayerEnter -= Chase;
        _activeArea.OnPlayerExit -= Idle;
    }

    void Update()
    {
        _state = StateController.CurrentEnum; // Inspector
    }

    void Idle()
    {
        if (GameState.CurrentState != GameState.Play) { return; }

        StateController.ChangeState(EnumState.Idle);
    }

    void Chase()
    {
        if (GameState.CurrentState != GameState.Play) { return; }

        StateController.ChangeState(EnumState.Chase);
    }
}
