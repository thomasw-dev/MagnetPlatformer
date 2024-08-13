public class EnemyChaseTrigger : PlayerTriggerEvents
{
    EnemyController GetController() => transform.parent.GetComponent<EnemyController>();

    void OnEnable()
    {
        OnPlayerEnter += GetController().EnterChase;
        OnPlayerStay += GetController().StayChase;
        OnPlayerExit += GetController().ExitChase;
    }

    void OnDisable()
    {
        OnPlayerEnter -= GetController().EnterChase;
        OnPlayerStay -= GetController().StayChase;
        OnPlayerExit -= GetController().ExitChase;
    }
}
