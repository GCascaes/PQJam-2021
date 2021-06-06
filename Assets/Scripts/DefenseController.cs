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

    private float defenseEnergyPercent = 100;
    private float defenseEnergyConsumptionPerFixedUpdate;
    private float defenseEnergyRestorePerFixedUpdate;

    private IEnumerator durationTrackerCoroutine;
    private IEnumerator cooldownTrackerCoroutine;

    private GameObject barrier;

    private GunController gunController;
    private IMovementController movementController;

    public bool IsDefending => isDefending;

    private void Start()
    {
        canDefend = defenseEnabled;

        defenseEnergyConsumptionPerFixedUpdate = 100 * Time.fixedDeltaTime / defenseDuration;
        defenseEnergyRestorePerFixedUpdate = 100 * Time.fixedDeltaTime / defenseCooldown;

        gunController = GetComponent<GunController>();
        movementController = GetComponent<IMovementController>();
    }

    public void StartDefending()
    {
        if (isDefending || !canDefend)
            return;

        SpawnBarrier();
        isDefending = true;

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

        cooldownTrackerCoroutine = DefenseCooldownTracker();
        StartCoroutine(cooldownTrackerCoroutine);
    }

    private IEnumerator DefenseDurationTracker()
    {
        do
        {
            defenseEnergyPercent -= defenseEnergyConsumptionPerFixedUpdate;
            yield return new WaitForFixedUpdate();
        }
        while (isDefending && defenseEnergyPercent > 0);

        StopDefending();
    }

    private IEnumerator DefenseCooldownTracker()
    {
        var previousCanDefendState = canDefend;
        canDefend = false;

        do
        {
            defenseEnergyPercent += defenseEnergyRestorePerFixedUpdate;
            yield return new WaitForFixedUpdate();
        }
        while (defenseEnergyPercent < 100);

        defenseEnergyPercent = 100;

        canDefend = previousCanDefendState;
    }

    private void SpawnBarrier()
    {
        var spawnPointTransform = barrierSpawnPoint != null ? barrierSpawnPoint.transform : transform;
        barrier = Instantiate(barrierPrefab, spawnPointTransform);
    }

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
