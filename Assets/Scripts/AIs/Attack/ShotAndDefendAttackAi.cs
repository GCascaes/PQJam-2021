using System.Collections;
using UnityEngine;

public class ShotAndDefendAttackAi : ShotAttackBaseAi, IAttackAi
{
    [SerializeField]
    private float defensePeriod;
    [SerializeField]
    private float idlePeriodBeforeShooting;
    [SerializeField]
    private float shootingPeriod;
    [SerializeField]
    private float idlePeriodAfterShooting;

    private DefenseController defenseController;

    protected override void Awake()
    {
        defenseController = GetComponent<DefenseController>();
        base.Awake();
    }

    protected override IEnumerator AttackRoutine()
    {
        while (IsEngaged)
        {
            defenseController.StartDefending();
            yield return new WaitForSecondsRealtime(defensePeriod);
            defenseController.StopDefending();

            yield return new WaitForSecondsRealtime(idlePeriodBeforeShooting);

            shootContinuously = true;
            Shoot();
            yield return new WaitForSecondsRealtime(shootingPeriod);
            shootContinuously = false;
            yield return new WaitForSecondsRealtime(idlePeriodAfterShooting);
        }
    }
}
