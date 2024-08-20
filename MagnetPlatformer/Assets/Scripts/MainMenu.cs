using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        GameState.ChangeState(GameState.Play);
    }
}
