using UnityEngine;

public class EnablePauseMenuOnStart : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenu;

    void Start()
    {
        _pauseMenu.SetActive(true);
    }
}
