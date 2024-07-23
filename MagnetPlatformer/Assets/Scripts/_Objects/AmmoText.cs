using TMPro;
using UnityEngine;

public class AmmoText : MonoBehaviour
{
    [SerializeField] MagnetWeapon _magnetWeapon;

    TMP_Text _ammoText;

    void Awake()
    {
        _ammoText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (_magnetWeapon.StateController.CurrentEnum == MagnetWeapon.StateEnum.Available)
        {
            _ammoText.color = Color.white;
            _ammoText.text = $"Ammo: {_magnetWeapon.Ammo}";
        }
        if (_magnetWeapon.StateController.CurrentEnum == MagnetWeapon.StateEnum.Cooldown)
        {
            _ammoText.color = Color.grey;
            _ammoText.text = $"Cooldown";
        }
        if (_magnetWeapon.StateController.CurrentEnum == MagnetWeapon.StateEnum.Reload)
        {
            _ammoText.color = Color.yellow;
            _ammoText.text = $"Reload";
        }
    }
}