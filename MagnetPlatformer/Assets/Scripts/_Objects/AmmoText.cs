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
        int ammo = _magnetWeapon.Ammo;
        string bullets = new string('•', ammo);

        if (_magnetWeapon.StateController.CurrentEnum == MagnetWeapon.StateEnum.Available)
        { 
            _ammoText.text = $"<size=28><color=white> \n<size=72><color=white><b>{ammo} {bullets}";
        }
        if (_magnetWeapon.StateController.CurrentEnum == MagnetWeapon.StateEnum.Cooldown)
        {
            _ammoText.text = $"<size=28><color=yellow>Cooldown\n<size=72><color=white><b>{ammo} {bullets}";

        }
        if (_magnetWeapon.StateController.CurrentEnum == MagnetWeapon.StateEnum.Refill)
        {
            _ammoText.text = $"<size=28><color=green>Refill\n<size=72><color=white><b>{ammo} {bullets}";
        }
    }
}