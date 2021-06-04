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
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] bool isPlayer;

    private float currentHealth;
    private bool isInvincible = false;

    private FlashController flashController;

    private void Awake()
    {
        currentHealth = maxHealth;
        flashController = GetComponent<FlashController>();
    }

    private void Start()
    {
        if (isPlayer)
            maxHealth = GameManager.instance.playerHealth;
    }
    public void TakeDamage(float damage)
    {
        if (isInvincible)
            return;

        currentHealth -= damage;
        if (shouldDie && currentHealth <= 0)
        {
            if(deathParticle)
                Instantiate(deathParticle.gameObject, transform);

            if (isPlayer)
                GameManager.instance.Death();
            Destroy(gameObject);

        }

        if (PlayerUI.instance != null)
            PlayerUI.instance.UpdateHeartBar(maxHealth, currentHealth);

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
