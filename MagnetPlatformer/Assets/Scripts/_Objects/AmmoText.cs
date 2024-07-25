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
            _ammoText.color = Color.white;
            _ammoText.text = $"Ammo: {_magnetWeapon.Ammo}\n<size=36><color=yellow>Cooldown";
        }
        if (_magnetWeapon.StateController.CurrentEnum == MagnetWeapon.StateEnum.Refill)
        {
            _ammoText.color = Color.white;
            _ammoText.text = $"Ammo: {_magnetWeapon.Ammo}\n<size=36><color=green>Refill";
        }
    }
}