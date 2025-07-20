using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class TimeLineEventHandler : MonoBehaviour
{
    public PlayableDirector firstTimeline;
    public PlayableDirector secondTimeline;
    private bool firstTimelinePlayed = false;
    private bool secondTimelinePlayed= false;

    // Trigger zone reference
    public Collider triggerZone;
    
    // Reference to the main camera and the timeline cameras
    public Camera mainCamera;
    public Camera firstTimelineCamera;
    public Camera secondTimelineCamera;

    void Start()
    {
        // Initially set the second timeline inactive
        secondTimeline.gameObject.SetActive(false);
        // Start the first timeline
        firstTimeline.Play();

        // Ensure the main camera is active and the timeline cameras are inactive
        mainCamera.gameObject.SetActive(true);
        firstTimelineCamera.gameObject.SetActive(false);
        secondTimelineCamera.gameObject.SetActive(false);
    }
    // Skip the first timeline when the button is clicked
    private void SkipTimeline()
    {
        firstTimelinePlayed = true;
        firstTimeline.Stop();

        // Deactivate the first timeline's camera
        firstTimelineCamera.gameObject.SetActive(false);

        // Reactivate the main camera
        mainCamera.gameObject.SetActive(true);
    }

    // Trigger the second timeline when the player enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !firstTimelinePlayed)
        {
            if(secondTimelinePlayed) return;
            TriggerSecondTimeline();
        }
    }

    // Activate the second timeline and its camera
    private void TriggerSecondTimeline()
    {
        secondTimelinePlayed = true;
        // Deactivate the main camera
        mainCamera.gameObject.SetActive(false);

        // Activate the second timeline's camera
        secondTimelineCamera.gameObject.SetActive(true);

        // Play the second timeline
        secondTimeline.gameObject.SetActive(true);
        secondTimeline.Play();
    }

    // When the second timeline finishes, return to the main camera
    public void OnSecondTimelineFinished()
    {
        mainCamera.gameObject.SetActive(true);
        secondTimelineCamera.gameObject.SetActive(false);
    }
}
