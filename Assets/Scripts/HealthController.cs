using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private bool isPlayer;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float invincibilityTime;
    [SerializeField]
    private float invincibleFlashPower;
    [SerializeField]
    private bool shouldDie;
    [SerializeField]
    private AudioClip deathAudio;
    [SerializeField]
    private float deathAudioVolume;
    [SerializeField]
    private float deathAudioPitch = 1;
    [SerializeField]
    private ParticleSystem deathParticle;
    [SerializeField]
    private bool destroyOnDeath = true;

    private float currentHealth;
    private bool isInvincible = false;

    private Coroutine invencibleCoroutine;

    private DefenseController defenseController;
    private FlashController flashController;

    private readonly List<Action> halfLifePercentHealthActions = new List<Action>();
    private readonly List<Action> quarterLifePercentHealthActions = new List<Action>();
    private readonly List<Action> lowLifePercentHealthActions = new List<Action>();

    private readonly List<Action> onDeathActions = new List<Action>();

    public enum LowHealthLevel
    {
        HalfLife,
        QuarterLife,
        LowLife,
    }

    private void Awake()
    {
        defenseController = GetComponent<DefenseController>();
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
        if (isInvincible || (defenseController != null && defenseController.IsDefending))
            return;

        var previousHealthPercent = 100 * currentHealth / maxHealth;
        currentHealth -= damage;
        var currentHealthPercent = 100 * currentHealth / maxHealth;
        
        InvokeLowHealthActions(previousHealthPercent, currentHealthPercent);

        if (shouldDie && currentHealth <= 0)
        {
            if (deathAudio)
                SoundManager.instance.PlaySFX(deathAudio, deathAudioVolume, deathAudioPitch);
            if (deathParticle)
                Instantiate(deathParticle.gameObject, transform.position, Quaternion.identity);

            if (isPlayer)
                GameManager.instance.Death();

            if (onDeathActions.Any())
            {
                foreach (var action in onDeathActions)
                    action.Invoke();
            }

            if (destroyOnDeath)
                Destroy(gameObject);
        }
        else
        {
            MakeInvincible(invincibilityTime);
        }

        UpdateUi();
    }

    public void MakeInvincible(float duration)
    {
        if (invencibleCoroutine != null)
            StopCoroutine(invencibleCoroutine);
        invencibleCoroutine = StartCoroutine(InvincibilityCooldown(duration));
    }

    public void RefillHealth(float amount)
    {
        if (currentHealth >= maxHealth)
            return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateUi();
    }
    
    public void SetMaxHealth(int value)
    {
        maxHealth = value;
        currentHealth = maxHealth;
        UpdateUi();
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

    public void RegisterDeathAction(Action action) => onDeathActions.Add(action);

    public void EndLevel()
    {
        isInvincible = true;
    }

    private IEnumerator InvincibilityCooldown(float duration)
    {
        isInvincible = true;

        if (flashController != null)
            flashController.Flash(duration, invincibleFlashPower);

        yield return new WaitForSecondsRealtime(duration);

        isInvincible = false;
    }
    
    private void InvokeLowHealthActions(float previousHealthPercent, float currentHealthPercent)
    {
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
    }

    private void UpdateUi()
    {
        if (PlayerUI.instance != null && isPlayer)
            PlayerUI.instance.UpdateHeartBar(maxHealth, currentHealth);
    }
}
