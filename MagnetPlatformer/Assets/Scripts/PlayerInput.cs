using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.EventSystems.PointerEventData;

public class UserInput : MonoBehaviour
{
    [SerializeField] MouseClick mouseClickArea;

    // Tunables
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
}