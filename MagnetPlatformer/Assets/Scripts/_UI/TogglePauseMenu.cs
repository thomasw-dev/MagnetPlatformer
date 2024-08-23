using UnityEngine;

public class TogglePauseMenu : MonoBehaviour
{
    CanvasGroup _canvasGroup;
    [SerializeField] bool _show = false;

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        GameState.Play.OnExit += () => Toggle(false);
    }

    void OnDisable()
    {
        GameState.Play.OnExit -= () => Toggle(false);
    }

    void Start()
    {
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

    void Toggle(bool state)
    {
        _show = state;

        if (_canvasGroup == null) { return; }

        _canvasGroup.alpha = state ? 1 : 0;
        _canvasGroup.interactable = state;
        _canvasGroup.blocksRaycasts = state;

        Player.AllowInput = !_show; // #%
    }
}
