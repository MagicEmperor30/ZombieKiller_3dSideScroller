using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int damage = 25;
    public GameObject hitEffectPrefab;
    public string targetTag = "Enemy";

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        CancelInvoke(); // in case it was scheduled previously
        Invoke(nameof(Deactivate), 2f); // auto-return after 2 seconds
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            if (other.TryGetComponent<ZombieHealth>(out var health))
                health.TakeDamage(damage);

            SpawnHitEffect();
            Deactivate();
            return;
        }

        if (other.TryGetComponent<ObstacleScript>(out var obstacle))
        {
            Vector3 pushDir = (other.transform.position - transform.position).normalized;
            obstacle.TakeDamage(damage, pushDir);

            SpawnHitEffect();
            Deactivate();
            return;
        }

        if (!other.isTrigger)
        {
            SpawnHitEffect();
            Deactivate();
        }
    }

    private void SpawnHitEffect()
    {
        if (hitEffectPrefab == null) return;

        // Optional: use a hit effect pool for better performance
        GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        if (effect.TryGetComponent<ParticleSystem>(out var ps))
            Destroy(effect, ps.main.duration + ps.main.startLifetime.constantMax);
        else
            Destroy(effect, 2f);
    }

    private void Deactivate()
    {
        rb.linearVelocity = Vector3.zero;
        BulletPool.Instance.ReturnBullet(gameObject);
    }
}
