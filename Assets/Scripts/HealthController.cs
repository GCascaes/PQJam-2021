using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField]
    private ParticleSystem deathParticle;
    [SerializeField]
    private bool isPlayer;

    private float currentHealth;
    private bool isInvincible = false;

    private Coroutine invencibleCoroutine;

    private FlashController flashController;

    private readonly List<Action> halfLifePercentHealthActions = new List<Action>();
    private readonly List<Action> quarterLifePercentHealthActions = new List<Action>();
    private readonly List<Action> lowLifePercentHealthActions = new List<Action>();

    public enum LowHealthLevel
    {
        HalfLife,
        QuarterLife,
        LowLife,
    }

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

    public void TakeDamagePercent(float akumaSpecialDamagePercent)
        => TakeDamage(currentHealth * (akumaSpecialDamagePercent / 100));

    public void TakeDamage(float damage)
    {
        if (isInvincible && isPlayer)
            return;

        var previousHealthPercent = 100 * currentHealth / maxHealth;
        currentHealth -= damage;
        var currentHealthPercent = 100 * currentHealth / maxHealth;

        if (previousHealthPercent > 50 && currentHealthPercent <= 50)
        {
            foreach (var action in halfLifePercentHealthActions)
                action.Invoke();
        }
        else if (previousHealthPercent > 25 && currentHealthPercent <= 25)
        {
            foreach (var action in quarterLifePercentHealthActions)
                action.Invoke();
        }
        else if (previousHealthPercent > 10 && currentHealthPercent <= 10)
        {
            foreach (var action in lowLifePercentHealthActions)
                action.Invoke();
        }

        if (shouldDie && currentHealth <= 0)
        {
            if (deathParticle)
                Instantiate(deathParticle.gameObject, transform.position, Quaternion.identity);

            if (isPlayer)
                GameManager.instance.Death();
            Destroy(gameObject);
        }
        else
        {
            MakeInvincible(invincibilityTime);
        }

        if (PlayerUI.instance != null && isPlayer)
            PlayerUI.instance.UpdateHeartBar(maxHealth, currentHealth);
    }

    public void MakeInvincible(float duration)
    {
        if (invencibleCoroutine != null)
            StopCoroutine(invencibleCoroutine);
        invencibleCoroutine = StartCoroutine(InvincibilityCooldown(duration));
    }

    public void RegisterLowHealthAction(LowHealthLevel healthLevel, Action action)
    {
        switch (healthLevel)
        {
            case LowHealthLevel.HalfLife:
                halfLifePercentHealthActions.Add(action);
                break;
            case LowHealthLevel.QuarterLife:
                quarterLifePercentHealthActions.Add(action);
                break;
            case LowHealthLevel.LowLife:
                lowLifePercentHealthActions.Add(action);
                break;
        }
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
