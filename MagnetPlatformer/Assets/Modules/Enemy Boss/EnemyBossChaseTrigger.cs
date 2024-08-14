public class EnemyBossChaseTrigger : PlayerTriggerEvents
{
    EnemyBossController GetController() => transform.parent.GetComponent<EnemyBossController>();

    void OnEnable()
    {
        OnPlayerEnter += GetController().EnterChase;
        OnPlayerStay += GetController().StayChase;
    }

    void OnDisable()
    {
        OnPlayerEnter -= GetController().EnterChase;
        OnPlayerStay -= GetController().StayChase;
    }
}
