using UnityEngine;

[System.Serializable]
public enum WeaponType
{
    Pistol,
    SMG
}

public class WeaponScript : MonoBehaviour
{
    [System.Serializable]
    public class WeaponData
    {
        public string name;
        public GameObject bulletPrefab;
        public float fireRate = 0.5f;
        public float bulletSpeed = 10f;
    }

    public Transform shootPoint;

    [Header("Weapon Settings")]
    public WeaponData pistol;
    public WeaponData submachineGun;

    private float nextFireTime;
    public WeaponType currentWeapon = WeaponType.Pistol;

    private bool facingRight = true;
    private Vector3 playerVelocity = Vector3.zero;

    // Call this from PlayerController
    public void Initialize(bool isFacingRight, Vector3 currentVelocity)
    {
        facingRight = isFacingRight;
        playerVelocity = currentVelocity;
    }

    public void TryShoot()
    {
        WeaponData weapon = GetCurrentWeapon();
        if (weapon == null || Time.time < nextFireTime) return;

        Vector3 spawnPos = shootPoint ? shootPoint.position : transform.position + Vector3.right;
        GameObject bullet = Instantiate(weapon.bulletPrefab, spawnPos, Quaternion.identity);

        if (bullet.TryGetComponent<Rigidbody>(out var bulletRb))
        {
            Vector3 shootDir = facingRight ? Vector3.right : Vector3.left;
            float totalSpeed = weapon.bulletSpeed + Vector3.Dot(playerVelocity, shootDir);
            bulletRb.linearVelocity = shootDir * totalSpeed;
        }

        Destroy(bullet, 2f);
        nextFireTime = Time.time + weapon.fireRate;
    }

    public void SwitchWeapon()
    {
        currentWeapon = currentWeapon == WeaponType.Pistol ? WeaponType.SMG : WeaponType.Pistol;
        Debug.Log("Switched to: " + currentWeapon);
    }

    private WeaponData GetCurrentWeapon()
    {
        return currentWeapon == WeaponType.Pistol ? pistol : submachineGun;
    }
}
