using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public enum ObstacleType { ExplosiveBarrel, HealthCrate }
    public ObstacleType type;

    [Header("Common Settings")]
    public int health = 1;
    public GameObject destroyEffect;
    public float pushForce = 5f;

    [Header("Explosive Barrel")]
    public float explosionRadius = 3f;
    public int explosionDamage = 100;
    public LayerMask explosionDamageLayers;

    [Header("Health Crate")]
    public int healAmount = 20;

    private Rigidbody rb;

    void Awake()
    {
            rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // Only explosive barrels react to trigger
        if (type != ObstacleType.ExplosiveBarrel) return;

        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            // Apply explosion damage
            Explode();

            // Spawn explosion visual effect
            if (destroyEffect)
                Instantiate(destroyEffect, transform.position, Quaternion.identity);

            // Optional: Push player
            if (other.attachedRigidbody != null)
            {
                Vector3 explosionDir = other.transform.position - transform.position;
                other.attachedRigidbody.AddForce(explosionDir.normalized * 10f, ForceMode.Impulse);
            }

            // Destroy the barrel
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int amount, Vector3 hitDirection = default)
    {
        health -= amount;

        // Apply knockback if Rigidbody exists
        if (rb != null && pushForce > 0f)
        {
            Vector3 force = hitDirection == Vector3.zero ? Vector3.back : hitDirection.normalized;
            rb.AddForce(force * pushForce, ForceMode.Impulse);
        }

        if (health <= 0)
        {
            HandleDestruction();
        }
    }

    private void HandleDestruction()
    {
        if (destroyEffect)
            Instantiate(destroyEffect, transform.position, Quaternion.identity);

        switch (type)
        {
            case ObstacleType.ExplosiveBarrel:
                Explode();
                break;
            case ObstacleType.HealthCrate:
                HealNearbyPlayer();
                break;
        }

        Destroy(gameObject);
    }

    private void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, explosionDamageLayers);
        foreach (var hit in hits)
        {
            // Look up in parent hierarchy for health components
            var healthComp = hit.GetComponentInParent<PlayerHealth>();
            if (healthComp != null)
            {
                Debug.Log("Damaging player: " + hit.name);
                healthComp.TakeDamage(explosionDamage);
                continue;
            }

            var zombieHealthComp = hit.GetComponentInParent<ZombieHealth>();
            if (zombieHealthComp != null)
            {
                Debug.Log("Damaging zombie: " + hit.name);
                zombieHealthComp.TakeDamage(explosionDamage);
                continue;
            }

            Debug.Log("No health component on: " + hit.name);
        }
    }


    private void HealNearbyPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 2f);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<PlayerHealth>(out var player))
            {
                player.Heal(healAmount);
                break;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (type == ObstacleType.ExplosiveBarrel)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
        else if (type == ObstacleType.HealthCrate)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 2f); // healing radius
        }
    }
}
