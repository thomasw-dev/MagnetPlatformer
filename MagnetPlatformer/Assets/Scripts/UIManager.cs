using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _startScreen;
    [SerializeField] GameObject _playingScreen;
    [SerializeField] GameObject _winScreen;

    void OnEnable()
    {
        GameManager.OnInitializationEnter += Initialize;
        GameManager.OnInitializationExit += DisableStartScreen;
        GameManager.OnPlayingEnter += EnablePlayingScreen;
        GameManager.OnPlayingExit += DisablePlayingScreen;
        GameManager.OnWinEnter += EnableWinScreen;
        GameManager.OnWinExit += DisableWinScreen;
    }

    void OnDisable()
    {
        GameManager.OnInitializationEnter -= Initialize;
        GameManager.OnInitializationExit -= DisableStartScreen;
        GameManager.OnPlayingEnter -= EnablePlayingScreen;
        GameManager.OnPlayingExit -= DisablePlayingScreen;
        GameManager.OnWinEnter -= EnableWinScreen;
        GameManager.OnWinExit -= DisableWinScreen;
    }

    void Initialize()
    {
        _startScreen.SetActive(true);
        _winScreen.SetActive(false);

    }

    void DisableStartScreen()
    {
        _startScreen.SetActive(false);
    }

    void EnablePlayingScreen()
    {
        // ...
    }

    void DisablePlayingScreen()
    {
        // ...
    }

    void EnableWinScreen()
    {
        _winScreen.SetActive(true);
    }

    void DisableWinScreen()
    {
        _winScreen.SetActive(false);
    }
}
