using System.Collections;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float invincibilityTime;
    [SerializeField]
    private float invincibleFlashPower;
    [SerializeField]
    private bool shouldDie;

    private float currentHealth;
    private bool isInvincible = false;

    private FlashController flashController;

    private void Awake()
    {
        currentHealth = maxHealth;
        flashController = GetComponent<FlashController>();
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible)
            return;

        currentHealth -= damage;
        if (shouldDie && currentHealth <= 0)
            Destroy(gameObject);

        StopAllCoroutines();
        StartCoroutine(InvincibilityCooldown(invincibilityTime));
    }

    public void MakeInvincible(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(InvincibilityCooldown(duration));
    }

    private IEnumerator InvincibilityCooldown(float duration)
    {
        isInvincible = true;

        if (flashController != null)
            flashController.Flash(duration, invincibleFlashPower);

        yield return new WaitForSecondsRealtime(duration);
        
        isInvincible = false;
    }
}
