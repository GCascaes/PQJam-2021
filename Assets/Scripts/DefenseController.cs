using System.Collections;
using UnityEngine;

public class DefenseController : MonoBehaviour
{
    [SerializeField]
    private GameObject barrierPrefab;
    [SerializeField]
    private GameObject barrierSpawnPoint;
    [SerializeField]
    private bool defenseEnabled;
    [SerializeField]
    private float defenseDuration;
    [SerializeField]
    private float defenseCooldown;
    [SerializeField]
    private bool defenseDisableShooting;
    [SerializeField]
    private bool defenseDisableMoving;

    private bool isDefending = false;
    private bool canDefend;
    private float defenseStartTime = 0;

    private IEnumerator durationTrackerCoroutine;
    private IEnumerator cooldownTrackerCoroutine;

    private GameObject barrier;

    private GunController gunController;
    private GroundMovementController movementController;

    private void Start()
    {
        canDefend = defenseEnabled;
        gunController = GetComponent<GunController>();
        movementController = GetComponent<GroundMovementController>();
    }

    public void StartDefending()
    {
        if (isDefending || !canDefend)
            return;

        SpawnBarrier();
        isDefending = true;
        defenseStartTime = Time.realtimeSinceStartup;

        if (defenseDisableShooting && gunController != null)
            gunController.DisableShooting();

        if (defenseDisableMoving && movementController != null)
            movementController.DisableMovement();

        TryStopCoroutine(durationTrackerCoroutine);
        TryStopCoroutine(cooldownTrackerCoroutine);

        durationTrackerCoroutine = DefenseDurationTracker();
        StartCoroutine(durationTrackerCoroutine);
    }

    public void StopDefending()
    {
        if (!isDefending)
            return;

        Destroy(barrier);
        isDefending = false;

        if (defenseDisableShooting && gunController != null)
            gunController.EnableShooting();

        if (defenseDisableMoving && movementController != null)
            movementController.EnableMovement();

        TryStopCoroutine(durationTrackerCoroutine);
        TryStopCoroutine(cooldownTrackerCoroutine);

        var cooldown = Mathf.Min(Time.realtimeSinceStartup - defenseStartTime, defenseCooldown);

        cooldownTrackerCoroutine = DefenseCooldownTracker(cooldown);
        StartCoroutine(cooldownTrackerCoroutine);
    }

    private IEnumerator DefenseDurationTracker()
    {
        yield return new WaitForSecondsRealtime(defenseDuration);
        StopDefending();
    }

    private IEnumerator DefenseCooldownTracker(float cooldown = 0)
    {
        var previousCanDefendState = canDefend;
        canDefend = false;

        if (cooldown <= 0)
            cooldown = defenseCooldown;

        yield return new WaitForSecondsRealtime(cooldown);

        canDefend = previousCanDefendState;
    }

    private void SpawnBarrier() => barrier = Instantiate(barrierPrefab, barrierSpawnPoint.transform);

    private bool TryStopCoroutine(IEnumerator coroutine)
    {
        if (coroutine is null)
            return false;

        try
        {
            StopCoroutine(coroutine);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
