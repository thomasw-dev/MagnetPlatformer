using TMPro;
using UnityEngine;

public class AmmoText : MonoBehaviour
{
    [SerializeField] MagnetGunController _magnetGun;

    TMP_Text _ammoText;

    void Awake()
    {
        _ammoText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        int ammo = _magnetGun.Values.Ammo;
        string bullets = new string('•', ammo);
        string ammoColor = ammo == 0 ? "red" : "white";

        if (_magnetGun.StateController.CurrentEnum == MagnetGunController.StateEnum.Available)
        { 
            _ammoText.text = $"<size=28><color=white> \n<size=72><color={ammoColor}>{ammo} {bullets}";
        }
        if (_magnetGun.StateController.CurrentEnum == MagnetGunController.StateEnum.Cooldown)
        {
            _ammoText.text = $"<size=28><color=yellow>Cooldown\n<size=72><color={ammoColor}>{ammo} {bullets}";

        }
        if (_magnetGun.StateController.CurrentEnum == MagnetGunController.StateEnum.Refill)
        {
            _ammoText.text = $"<size=28><color=green>Refill\n<size=72><color={ammoColor}>{ammo} {bullets}";
        }
    }
}