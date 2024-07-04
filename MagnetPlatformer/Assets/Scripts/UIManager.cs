using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _winScreen;

    void OnEnable()
    {
        GameManager.OnPlayingEnter += EnablePlayingUI;
        GameManager.OnPlayingExit += DisablePlayingUI;
        GameManager.OnWinEnter += EnableWinUI;
        GameManager.OnWinExit += DisableWinUI;
    }

    void OnDisable()
    {
        GameManager.OnPlayingEnter -= EnablePlayingUI;
        GameManager.OnPlayingExit -= DisablePlayingUI;
        GameManager.OnWinEnter -= EnableWinUI;
        GameManager.OnWinExit -= DisableWinUI;
    }

    void EnablePlayingUI()
    {
        _winScreen.SetActive(false);
    }

    void DisablePlayingUI()
    {
        _winScreen.SetActive(false);
    }

    void EnableWinUI()
    {
        _winScreen.SetActive(true);
    }

    void DisableWinUI()
    {
        _winScreen.SetActive(false);
    }
}
