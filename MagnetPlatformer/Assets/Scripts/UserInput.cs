using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.EventSystems.PointerEventData;

public class UserInput : MonoBehaviour
{
    [SerializeField] MouseClick mouseClickArea;
    
    private const InputButton POSITIVE_CHARGE = InputButton.Left;
    private const InputButton NEGATIVE_CHARGE = InputButton.Right;

    public static float MoveHorizontal;
    public static float MoveRight;

    private void Update()
    {
        MoveHorizontal = Input.GetAxis("Horizontal");
    }

    public void OnMouseClick()
    {
        InputButton click = mouseClickArea.eventData.button;
        if (click == POSITIVE_CHARGE) {  }
        if (click == NEGATIVE_CHARGE) {  }
    }

    public static bool Jump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public static bool IsMovingLeft()
    {
        return Input.GetKey(KeyCode.A);
    }

    public static bool IsMovingRight()
    {
        return Input.GetKey(KeyCode.D);
    }
}