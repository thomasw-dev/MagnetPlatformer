using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _startScreen;
    [SerializeField] GameObject _playingScreen;
    [SerializeField] GameObject _winScreen;
    [SerializeField] GameObject _loseScreen;

    [SerializeField] List<GameObject> _screens;

    void OnEnable()
    {
        GameState.Initialize.OnEnter += Initialize;
        GameState.Initialize.OnExit += DisableStartScreen;
        GameState.Play.OnEnter += EnablePlayingScreen;
        GameState.Play.OnExit += DisablePlayingScreen;
        GameState.Win.OnEnter += EnableWinScreen;
        GameState.Win.OnExit += DisableWinScreen;
        GameState.Lose.OnEnter += EnableLoseScreen;
        GameState.Lose.OnExit += DisableLoseScreen;
    }

    void OnDisable()
    {
        GameState.Initialize.OnEnter -= Initialize;
        GameState.Initialize.OnExit -= DisableStartScreen;
        GameState.Play.OnEnter -= EnablePlayingScreen;
        GameState.Play.OnExit -= DisablePlayingScreen;
        GameState.Win.OnEnter -= EnableWinScreen;
        GameState.Win.OnExit -= DisableWinScreen;
        GameState.Lose.OnEnter -= EnableLoseScreen;
        GameState.Lose.OnExit -= DisableLoseScreen;
    }

    void SoloScreen(List<GameObject> list, GameObject target)
    {
        if (list.Contains(target))
        {
            foreach (GameObject obj in list)
            {
                obj.SetActive(obj == target);
            }
        }
        else Debug.Log("Target object not found in the list.");
    }

    [ContextMenu("Enable All Screens")]
    void EnableAllScreens()
    {
        foreach (GameObject obj in _screens)
            obj.SetActive(true);
    }

    void Initialize()
    {
        EnableStartScreen();
    }

    [ContextMenu("Solo: Start Screen")]
    void EnableStartScreen() => SoloScreen(_screens, _startScreen);
    void DisableStartScreen() => _startScreen.SetActive(false);

    [ContextMenu("Solo: Playing Screen")]
    void EnablePlayingScreen() => SoloScreen(_screens, _playingScreen);
    void DisablePlayingScreen() => _playingScreen.SetActive(false);

    [ContextMenu("Solo: Win Screen")]
    void EnableWinScreen() => SoloScreen(_screens, _winScreen);
    void DisableWinScreen() => _winScreen.SetActive(false);

    [ContextMenu("Solo: Lose Screen")]
    void EnableLoseScreen() => SoloScreen(_screens, _loseScreen);
    void DisableLoseScreen() => _loseScreen.SetActive(false);
}
