using UnityEngine;
using UnityEngine.UI;
public class ZombieHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100; // Maximum health of the zombie
    public int currentHealth;    // Current health of the zombie
    public Slider zombieHealthSlider; // UI Slider for health bar

    [Header("Damage Settings")]
    public float invincibilityDuration = 0.5f; // Time period to prevent taking multiple hits
    private float lastDamageTime = -999f;      // Time of last damage received

    private ZombieAI zombieAI; // Reference to the ZombieAI component

    private void Start()
    {
        currentHealth = maxHealth;  // Initialize health to max
        zombieAI = GetComponent<ZombieAI>(); // Get the ZombieAI component
        if (zombieHealthSlider != null)
        {
            zombieHealthSlider.maxValue = maxHealth; // Set the slider's max value to max health
            zombieHealthSlider.value = currentHealth; // Set the initial health value to current health
        }
    }

    private void UpdateHealthUI()
    {
        // Update health UI if the slider is assigned
        if (zombieHealthSlider != null)
        {
            zombieHealthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        // Check if the invincibility duration has passed
        if (Time.time - lastDamageTime < invincibilityDuration)
            return;

        lastDamageTime = Time.time;  // Update last damage time

        currentHealth -= amount; // Subtract the damage amount from current health

        // Force the zombie to chase the player when damaged
        zombieAI.isChasing = true;
        zombieAI.TransitionToState(zombieAI.walkState);

        // If health goes below or equal to 0, the zombie dies
        if (currentHealth <= 0)
        {
            Die();
        }

        // Update health UI
        UpdateHealthUI();
    }

    public bool IsEnraged()
    {
        // Zombie becomes enraged if its health is less than or equal to half of its max health
        return currentHealth <= maxHealth / 2;
    }

    private void Die()
    {
        // Trigger the zombie's death behavior
        zombieAI.isDead = true;
        zombieAI.TriggerDeathAnimation();

        // Destroy the zombie object after a short delay (for death animation)
        Destroy(gameObject, 2f);
    }
}
