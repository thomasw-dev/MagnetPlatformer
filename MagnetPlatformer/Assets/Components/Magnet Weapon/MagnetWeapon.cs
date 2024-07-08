using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetWeapon : MonoBehaviour
{
    [SerializeField] Transform _attachTo;

    public static Magnet.Charge CurrentCharge = Magnet.Charge.Neutral;
    [SerializeField] Magnet.Charge _currentCharge; // Inspector

    [SerializeField] Sprite[] _magnetSprites;
    SpriteRenderer _spriteRenderer;

    bool _isInputEnabled = true;

    void OnEnable()
    {
        InputManager.OnMagnetWeaponSetCharge += InputSetCharge;
        GameManager.OnPlayingExit += Restore;
    }

    void OnDisable()
    {
        InputManager.OnMagnetWeaponSetCharge -= InputSetCharge;
        GameManager.OnPlayingExit -= Restore;
    }

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // _isInputEnabled is only true when GameState is Playing
        _isInputEnabled = GameManager.GameState == GameManager.State.Playing ? true : false;

        _currentCharge = CurrentCharge;

        AimSelfAtCursor();
        AttachSelfToPlayer();
    }

    void AimSelfAtCursor()
    {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.Rotate(0f, 0f, -90f); // left side -> top side aiming
    }

    void AttachSelfToPlayer()
    {
        transform.position = _attachTo.position;
    }

    void SetCharge(Magnet.Charge charge)
    {
        CurrentCharge = charge;
        SetMagnetSprite(charge);
    }

    void InputSetCharge(Magnet.Charge charge)
    {
        if (!_isInputEnabled) { return; }
        CurrentCharge = charge;
        SetMagnetSprite(charge);
    }

    void SetMagnetSprite(Magnet.Charge charge)
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

    void Restore() => SetCharge(Magnet.Charge.Neutral);
}
