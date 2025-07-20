using UnityEngine;
using UnityEngine.EventSystems;

public class HoldButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ButtonType { Left, Right, Run, Brake}
    public ButtonType buttonType;

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (buttonType)
        {
            case ButtonType.Left:
                MobileInput.Instance.isMovingLeft = true;
                break;
            case ButtonType.Right:
                MobileInput.Instance.isMovingRight = true;
                break;
            case ButtonType.Run:
                MobileInput.Instance.isRunning = true;
                break;
            case ButtonType.Brake:
                MobileInput.Instance.isBraking = true;
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch (buttonType)
        {
            case ButtonType.Left:
                MobileInput.Instance.isMovingLeft = false;
                break;
            case ButtonType.Right:
                MobileInput.Instance.isMovingRight = false;
                break;
            case ButtonType.Run:
                MobileInput.Instance.isRunning = false;
                break;
            case ButtonType.Brake:
                MobileInput.Instance.isBraking = false;
                break;
        }
    }
}
