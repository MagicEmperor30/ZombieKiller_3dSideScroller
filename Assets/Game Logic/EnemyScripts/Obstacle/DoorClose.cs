using UnityEngine;

public class DoorClose : MonoBehaviour
{
    public enum DoorState
    {
        Open,
        Close
    }

    [Header("Door Settings")]
    public DoorState currentState = DoorState.Close;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            switch (currentState)
            {
                case DoorState.Open:
                    animator.Play("DoorOpeningAnim");
                    break;

                case DoorState.Close:
                    animator.Play("DoorClosingAnim"); 
                    break;
            }
        }

    }
}
