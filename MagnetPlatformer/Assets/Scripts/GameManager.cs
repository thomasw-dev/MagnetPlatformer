using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameStates GameState;
    public enum GameStates
    {
        Initialization, Playing, Win, Lose
    }

    void Start()
    {
        SetGameState(GameStates.Playing); 
    }

    void SetGameState(GameStates gameState)
    {
        GameState = gameState;
        Debug.Log($"Game State: {GameState}");
    }
}
