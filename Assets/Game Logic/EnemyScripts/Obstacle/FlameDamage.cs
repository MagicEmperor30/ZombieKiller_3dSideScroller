using System.Collections;
using UnityEngine;

public class FlameDamage : MonoBehaviour
{
    public float damagePerTick = 10f;
    public float damageInterval = 1f;
    public Collider flameDamageZone; // The collider that defines the damage area

    private bool isPlayerInFlame = false;
    private PlayerHealth playerHealth;
    private Coroutine damageCoroutine;
    private FlameThrower flameThrower;

    private void Start()
    {
        flameThrower = GetComponentInChildren<FlameThrower>(); // Reference to FlameThrower script
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth = other.GetComponent<PlayerHealth>();

            // Start damage coroutine only if flame is active
            if (playerHealth != null && flameThrower.isFlameActive)
            {
                if (damageCoroutine != null)
                    StopCoroutine(damageCoroutine);

                damageCoroutine = StartCoroutine(ApplyDamage());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Stop damage coroutine when player leaves the flame area
            if (damageCoroutine != null)
                StopCoroutine(damageCoroutine);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Only deal damage if the flame is active
        if (other.CompareTag("Player") && flameThrower.isFlameActive)
        {
            if (playerHealth != null && damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(ApplyDamage());
            }
        }
    }

    private IEnumerator ApplyDamage()
    {
        while (flameThrower.isFlameActive)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage((int)damagePerTick);
            }
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
