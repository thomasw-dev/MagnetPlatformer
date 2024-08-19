public interface IWinCondition
{
    void Win()
    {
        GameState.ChangeState(GameState.Win);
    }
}
