using UnityEngine;

public class TogglePauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] bool _show = false;

    void Start()
    {
        _show = false;
        Toggle(_show);
    }

    void Update()
    {

    }

    void Toggle(bool state)
    {

    }
}
