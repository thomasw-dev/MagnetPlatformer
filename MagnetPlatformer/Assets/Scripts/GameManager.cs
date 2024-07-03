using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameStates
    {
        Initialization, Playing, Win, Lose
    }
    public static GameStates GameState;

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
