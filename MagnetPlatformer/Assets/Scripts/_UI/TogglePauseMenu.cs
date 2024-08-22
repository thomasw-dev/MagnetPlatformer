using UnityEngine;

public class TogglePauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] bool _show = false;

    void OnEnable()
    {
        GameState.Play.OnExit += ToggleFalse;
    }

    void OnDisable()
    {
        GameState.Play.OnExit += ToggleFalse;
    }

    void Start()
    {
        _pauseMenu = transform.Find("Pause Menu").gameObject; // #%
        Toggle(false);
    }

    void Update()
    {
        if (GameState.CurrentState != GameState.Play) { return; }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle(!_show);
        }
    }

    void ToggleFalse() => Toggle(false);

    void Toggle(bool state)
    {
        _show = state;
        _pauseMenu.SetActive(_show);

        Player.AllowInput = !_show; // #%
    }
}
