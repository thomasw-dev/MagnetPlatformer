using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class Magnet : MonoBehaviour
{
    [SerializeField] Transform _attachTo;

    public enum Charges { Neutral, Positive, Negative }
    public static Charges CurrentCharge = Charges.Neutral;
    [SerializeField] Charges _currentCharge;

    void OnEnable()
    {
        UserInput.MagnetSetChargePositive += SetChargePositive;
        UserInput.MagnetSetChargeNegative += SetChargeNegative;
    }
    void OnDisable()
    {
        UserInput.MagnetSetChargePositive -= SetChargePositive;
        UserInput.MagnetSetChargeNegative -= SetChargeNegative;
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

    void SetChargeNeutral()
    {
        CurrentCharge = Charges.Neutral;
        Debug.Log("SetChargeNeutral");
    }
    void SetChargePositive()
    {
        CurrentCharge = Charges.Positive;
        Debug.Log("SetChargePositive");
    }
    void SetChargeNegative()
    {
        CurrentCharge = Charges.Negative;
        Debug.Log("SetChargeNegative");
    }
}
