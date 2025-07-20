using TMPro;
using UnityEngine;

public class CompleteMissionTrigger : MonoBehaviour
{
    [Header("Complete Event")]
    public GameObject completeEvent;

    private void OnCollisionEnter(Collision collision)
    {
        // This will trigger the event when any collider enters the trigger zone
        if (collision.collider != null)
        {
            TriggerCompleteEvent();
        }
    }

    private void TriggerCompleteEvent()
    {
        if (completeEvent != null)
        {
            completeEvent.SetActive(true); // Activate the complete event
        }
    }
}
