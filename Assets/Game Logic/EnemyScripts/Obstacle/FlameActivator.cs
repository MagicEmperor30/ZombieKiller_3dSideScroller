using System.Collections;
using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    [Header("Flame Settings")]
    public ParticleSystem flameEffect;
    public float flameActiveDuration = 2f; // Duration for which flames will be on
    public float flameCooldownDuration = 3f; // Duration for which flames will be off

    [Header("Flame Zones")]
    public Collider flameTriggerZone; // Trigger zone for flame

    public bool isFlameActive = false;
    private Coroutine flameRoutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Start the flame activation coroutine if player enters the zone
            if (flameRoutine == null)
                flameRoutine = StartCoroutine(ActivateFlames());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Stop the flame activation routine if player exits the zone
            if (flameRoutine != null)
            {
                StopCoroutine(flameRoutine);
                flameRoutine = null;
            }

            DeactivateFlames();
        }
    }

    private IEnumerator ActivateFlames()
    {
        while (true)
        {
            Debug.Log("Flame Is Active");
            // Flame on for the active duration
            isFlameActive = true;
            flameEffect.Play();
            yield return new WaitForSeconds(flameActiveDuration);

            // Flame off for the cooldown duration
            isFlameActive = false;
            flameEffect.Stop();
            yield return new WaitForSeconds(flameCooldownDuration);
        }
    }

    private void DeactivateFlames()
    {
        isFlameActive = false;
        if (flameEffect.isPlaying)
            flameEffect.Stop();
    }
}
