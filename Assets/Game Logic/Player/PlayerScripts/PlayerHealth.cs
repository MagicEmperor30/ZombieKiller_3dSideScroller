using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Invincibility Settings")]
    public float invincibleDuration = 1f;
    private bool isInvincible;
    private float invincibilityTimer;
    
    [Header("UI Elements")]
    public Slider healthBar;
    public TMP_Text healthText;
    public Image damageIndicator;
    public Image DeathImage;

    [Header("Events")]
    public UnityEvent onTakeDamage;
    public UnityEvent onDeath;
    public SceneManagerMobile sceneManagerMobile;
    private PlayerController controller;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        currentHealth = maxHealth;
        UpdateUI();
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
                isInvincible = false;
        }
    }

    public void TakeDamage(int amount)
    {
        if (controller != null && controller.IsDefending())
        {
            Debug.Log("Blocked Damage due to Shield!");
            return;
        }

        if (isInvincible)
            return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
        controller.animator.SetTrigger("Hit");

        if (damageIndicator != null)
        {
            damageIndicator.gameObject.SetActive(true);
            StopCoroutine("ShowDamageImage");
            StartCoroutine(ShowDamageImage(0.5f)); // Adjust duration if needed
        }

        onTakeDamage?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            isInvincible = true;
            invincibilityTimer = invincibleDuration;
        }
    }

    private void UpdateUI()
    {
        if (healthBar != null)
            healthBar.value = GetHealthPercent();

        if (healthText != null)
            healthText.text = $"{currentHealth} %";
    }

    IEnumerator ShowDamageImage(float duration)
    {
        float fadeInTime = 0.2f;
        float holdTime = duration;
        float fadeOutTime = 0.5f;

        Color startColor = damageIndicator.color;
        Color tempColor = startColor;

        // Fade in from 50% to 100%
        float t = 0;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0.5f, 1f, t / fadeInTime);
            tempColor.a = alpha;
            damageIndicator.color = tempColor;
            yield return null;
        }

        // Hold at full alpha
        tempColor.a = 1f;
        damageIndicator.color = tempColor;
        yield return new WaitForSeconds(holdTime);

        // Fade out to 0
        t = 0;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeOutTime);
            tempColor.a = alpha;
            damageIndicator.color = tempColor;
            yield return null;
        }

        damageIndicator.gameObject.SetActive(false);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
    }

    public void Die()
    {
        controller.isDead = true;
        Debug.Log($"{gameObject.name} died!");
        onDeath?.Invoke();
        StartCoroutine(HandleDeathSequence());
    }

    private IEnumerator HandleDeathSequence()
    {
        yield return new WaitForSeconds(1f); // Delay before showing death screen
        controller.animator.SetTrigger("Dead");

        if (DeathImage != null)
        {
            DeathImage.gameObject.SetActive(true);
            yield return null;
        }

        // Halt player movement
        DisablePlayerControl();

        // Wait for a brief moment before restarting the game
        yield return new WaitForSeconds(1f);

        // Restart the game after the death animation
        sceneManagerMobile.RestartGame();
    }

    private void DisablePlayerControl()
    {
        MonoBehaviour movementScript = GetComponent<PlayerController>();
        StartCoroutine(DelayCollider(1));
        if (movementScript != null)
            movementScript.enabled = false;

        // You can also disable input if using Input System
        var input = GetComponent<PlayerInput>();
        if (input != null)
            input.enabled = false;
    }

    private IEnumerator DelayCollider(int delay)
    {
        yield return new WaitForSeconds(delay);
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.enabled = false;
    }

    public bool IsAlive() => currentHealth > 0;
    public int GetHealth() => currentHealth;
    public float GetHealthPercent() => (float)currentHealth / maxHealth;
}
