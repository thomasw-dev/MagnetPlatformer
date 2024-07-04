using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class MagnetWeapon : MonoBehaviour
{
    [SerializeField] Transform _attachTo;
    [SerializeField] Sprite[] _magnetSprites;
    SpriteRenderer _spriteRenderer;

    public static Magnet.Charge CurrentCharge = Magnet.Charge.Neutral;
    [SerializeField] Magnet.Charge _currentCharge;

    void OnEnable()
    {
        UserInput.OnMagnetSetCharge += SetCharge;
    }
    void OnDisable()
    {
        UserInput.OnMagnetSetCharge -= SetCharge;
    }

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Inspector
        _currentCharge = CurrentCharge;

        if (GameState == GameStates.Playing)
        {
            AimSelfAtCursor();
            AttachSelfToPlayer();
        }
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
