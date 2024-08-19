using System;

public class EnemyBossRoomDoorForceEntryEnterTrigger : PlayerTriggerEvents
{
    public event Action OnPlayerExitRight;

    void OnEnable()
    {
        OnPlayerExit += CheckPlayerExit;
    }

    void OnDisable()
    {
        OnPlayerExit -= CheckPlayerExit;
    }

    void CheckPlayerExit()
    {
        if (Method.GetPlayerObject().transform.position.x > transform.position.x)
        {
            OnPlayerExitRight?.Invoke();
        }
    }
}
