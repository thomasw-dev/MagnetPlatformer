using UnityEngine;

public class MagnetGunVisual : MonoBehaviour
{
    [Header("Dependencies")]
    // These fields are required to be assigned in order for this module to function.

    public MagnetGunController _magnetGunController;

    [Header("Sprites")]
    [SerializeField] Sprite[] _magnetSprites;

    SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        _magnetGunController.OnCurrentChargeChanged += ChangeMagnetSpriteByCharge;
    }

    void OnDisable()
    {
        _magnetGunController.OnCurrentChargeChanged -= ChangeMagnetSpriteByCharge;
    }

    void ChangeMagnetSpriteByCharge(Magnet.Charge charge)
    {
        _spriteRenderer.sprite = GetMagnetSpriteByCharge(charge);
        Sprite GetMagnetSpriteByCharge(Magnet.Charge charge) => charge switch
        {
            Magnet.Charge.Neutral => _magnetSprites[0],
            Magnet.Charge.Positive => _magnetSprites[1],
            Magnet.Charge.Negative => _magnetSprites[2],
            _ => _magnetSprites[0]
        };
    }
}
