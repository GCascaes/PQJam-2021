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
    Coroutine invencibleCoroutine;

    private FlashController flashController;

    private void Awake()
    {
        flashController = GetComponent<FlashController>();
    }

    private void Start()
    {
        if (isPlayer)
            maxHealth = GameManager.instance.playerHealth;
        currentHealth = maxHealth;
    }
    public void TakeDamage(float damage)
    {
        if (isInvincible && isPlayer)
            return;

        currentHealth -= damage;
        if (shouldDie && currentHealth <= 0)
        {
            if (deathParticle)
                Instantiate(deathParticle.gameObject, transform.position, Quaternion.identity);

            if (isPlayer)
                GameManager.instance.Death();
            Destroy(gameObject);

        }
        else
            MakeInvincible(invincibilityTime);

        if (PlayerUI.instance != null && isPlayer)
            PlayerUI.instance.UpdateHeartBar(maxHealth, currentHealth);

        //StopAllCoroutines();
        //StartCoroutine(InvincibilityCooldown(invincibilityTime));
    }

    public void MakeInvincible(float duration)
    {
        if (invencibleCoroutine != null)
            StopCoroutine(invencibleCoroutine);
        //StopAllCoroutines();
        invencibleCoroutine = StartCoroutine(InvincibilityCooldown(duration));
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
