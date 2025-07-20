using UnityEngine;

public class DamageScript : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damageAmount = 10; // Customize per object

    private void OnTriggerStay(Collider other)
    {
        // Try to get a PlayerHealth component on the other object
        PlayerHealth targetHealth = other.GetComponent<PlayerHealth>();

        if (targetHealth != null && targetHealth.IsAlive())
        {
            targetHealth.TakeDamage(damageAmount);
        }
    }
}
