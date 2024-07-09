using UnityEngine;
using UnityEngine.Tilemaps;

public class EnvironmentTilemap : MonoBehaviour
{
    [SerializeField] Tilemap _deathTilemap;

    void OnEnable()
    {
        GameState.Initialize.OnEnter += HideDeathTilemap;
    }

    void OnDisable()
    {
        GameState.Initialize.OnEnter -= HideDeathTilemap;
    }

    void HideDeathTilemap()
    {
        _deathTilemap.color = new Color(1, 1, 1, 0);
    }
}
