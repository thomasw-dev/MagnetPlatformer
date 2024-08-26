using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] LevelProgressCollection _levelProgressCollection;

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
        _levelProgressCollection.ResetAll();
    }
}
