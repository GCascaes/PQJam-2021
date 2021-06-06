using System.Collections;
using UnityEngine;

public class BossAi : MonoBehaviour
{
    [SerializeField]
    [Range(0, 100)]
    private int punchChance;
    [SerializeField]
    private float punchPrepareTime;
    [SerializeField]
    private float punchEndTime;
    [SerializeField]
    private float punchDamage;
    [SerializeField]
    private float hadoukenInterval;
    [SerializeField]
    [Range(0, 100)]
    private int tauntChance;
    [SerializeField]
    private float tauntDuration;
    [SerializeField]
    private float akumaSpecialDuration;
    [SerializeField]
    private float akumaSpecialDamagePercent;
    [SerializeField]
    private GameObject akumaSpecialParticlePrefab;
    [SerializeField]
    private float idleTimeBetweenAttacks;
    [SerializeField]
    private float lowHealthVelocityIncreasePercent;

    private Animator animator;
    private HealthController healthController;
    private GunController gunController;
    private ContactDamageController contactDamageController;
    private GroundMovementController movementController;

    private GameObject target;

    private IFollowMovementAi followMovementAi;

    private bool bossStarted = false;
    private bool akumaSpecialAllowed = false;
    private bool tryingAkumaSpecial = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        healthController = GetComponent<HealthController>();
        gunController = GetComponent<GunController>();
        contactDamageController = GetComponent<ContactDamageController>();
        movementController = GetComponent<GroundMovementController>();

        followMovementAi = GetComponent<IFollowMovementAi>();

        healthController.RegisterLowHealthAction(
            HealthController.LowHealthLevel.HalfLife,
            () => LevelUp());
        healthController.RegisterLowHealthAction(
            HealthController.LowHealthLevel.QuarterLife,
            () => LevelUp());
        healthController.RegisterLowHealthAction(
            HealthController.LowHealthLevel.QuarterLife,
            () => LevelUp(enableSpecial: true));

        healthController.RegisterDeathAction(() => Death());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckStartBoss(collision.gameObject);
        CheckAkumaSpecial(collision);
    }

    public void TryStartBoss()
        => CheckStartBoss(GameObject.FindGameObjectWithTag("Player"));

    private void LevelUp(bool enableSpecial = false)
    {
        movementController.IncreaseVelocity(lowHealthVelocityIncreasePercent);
        idleTimeBetweenAttacks *= 1 - lowHealthVelocityIncreasePercent / 100;

        if (enableSpecial)
            akumaSpecialAllowed = true;
    }

    private void CheckStartBoss(GameObject target)
    {
        if (bossStarted || !target.CompareTag("Player"))
            return;

        this.target = target;

        if (followMovementAi != null)
            followMovementAi.FollowTarget(target);

        StartCoroutine(BossFightAi());
    }

    private void CheckAkumaSpecial(Collider2D collision)
    {
        if (!akumaSpecialAllowed || !tryingAkumaSpecial || !collision.CompareTag("Player"))
            return;

        if (!collision.IsTouching(contactDamageController.ContactDamageCollider))
            return;

        akumaSpecialAllowed = false;

        AkumaSpecial();
    }

    private IEnumerator BossFightAi()
    {
        bossStarted = true;

        animator.SetBool("IntroPlaying", true);
        yield return new WaitForSecondsRealtime(3);
        animator.SetBool("IntroPlaying", false);

        while (true)
        {
            var attackChance = Random.Range(0, 100);

            switch (attackChance)
            {
                case var _ when akumaSpecialAllowed && ShouldPunch():
                    yield return Punch();
                    break;
                case var _ when attackChance < tauntChance:
                    yield return Taunt();
                    break;
                case var _ when attackChance > punchChance:
                    yield return Hadouken();
                    break;
                case var _ when ShouldPunch():
                    yield return Punch();
                    break;
                default:
                    yield return Hadouken();
                    break;
            }

            yield return new WaitForSecondsRealtime(idleTimeBetweenAttacks);
        }
    }

    private IEnumerator Taunt()
    {
        animator.SetTrigger("Taunt");
        yield return new WaitForSecondsRealtime(tauntDuration);
    }

    private bool ShouldPunch()
    {
        if (target is null)
            return false;

        return Mathf.Abs(target.transform.position.x - transform.position.x) < movementController.DashDistance;
    }

    private IEnumerator Punch()
    {
        animator.SetBool("Punch", true);
        yield return new WaitForSecondsRealtime(punchPrepareTime);
        
        movementController.Dash();
        healthController.MakeInvincible(movementController.DashTime);

        if (akumaSpecialAllowed)
        {
            contactDamageController.AlterDamage(akumaSpecialDamagePercent, movementController.DashTime, isPercentual: true);
            tryingAkumaSpecial = true;
        }
        else
        {
            contactDamageController.AlterDamage(punchDamage, movementController.DashTime);
        }

        yield return new WaitForSecondsRealtime(movementController.DashTime);

        tryingAkumaSpecial = false;
        animator.SetBool("Punch", false);
        yield return new WaitForSecondsRealtime(punchEndTime);
    }

    private IEnumerator Hadouken()
    {
        gunController.Shoot();
        yield return new WaitForSecondsRealtime(hadoukenInterval);
        movementController.SmoothMove(0, true, false);
        gunController.Shoot();
        yield return new WaitForSecondsRealtime(hadoukenInterval);
        gunController.Shoot();
    }

    private void AkumaSpecial()
        => Instantiate(akumaSpecialParticlePrefab, transform.position, transform.rotation);

    private void Death()
    {
        StopAllCoroutines();
        followMovementAi.StopFollowing();
        contactDamageController.enabled = false;
        animator.SetBool("Defeat", true);
    }
}
