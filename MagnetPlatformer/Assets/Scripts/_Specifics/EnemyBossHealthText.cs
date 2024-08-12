using TMPro;
using UnityEngine;

public class EnemyBossHealthText : MonoBehaviour
{
    [SerializeField] EnemyBossHealth _enemyBossHealth;

    TMP_Text _tmpText;

    void Awake()
    {
        _tmpText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _tmpText.text = $"Health: {_enemyBossHealth.CurrentHealth}";
    }
}
