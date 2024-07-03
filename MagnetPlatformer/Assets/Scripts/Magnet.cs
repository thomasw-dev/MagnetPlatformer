using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class Magnet : MonoBehaviour
{
    [SerializeField] Transform _attachTo;
    [SerializeField] Sprite[] _magnetSprites;
    SpriteRenderer _spriteRenderer;

    public enum Charges { Neutral, Positive, Negative }
    public static Charges CurrentCharge = Charges.Neutral;
    [SerializeField] Charges _currentCharge;

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

    void SetCharge(Charges charge)
    {
        CurrentCharge = charge;
        _spriteRenderer.sprite = GetMagnetSpriteByCharge(charge);
        Sprite GetMagnetSpriteByCharge(Charges charge) => charge switch
        {
            Charges.Neutral => _magnetSprites[0],
            Charges.Positive => _magnetSprites[1],
            Charges.Negative => _magnetSprites[2],
            _ => _magnetSprites[0]
        };
    }
}
