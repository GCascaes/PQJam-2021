using System;
using System.Collections;
using UnityEngine;

public class BossAi : MonoBehaviour
{
    [SerializeField]
    private float punchPrepareTime;
    [SerializeField]
    private float punchEndTime;

    private Animator animator;
    private HealthController healthController;
    private GunController gunController;
    private ContactDamageController contactDamageController;
    private GroundMovementController movementController;
    
    private IFollowMovementAi followMovementAi;

    private bool bossStarted = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        healthController = GetComponent<HealthController>();
        gunController = GetComponent<GunController>();
        contactDamageController = GetComponent<ContactDamageController>();
        movementController = GetComponent<GroundMovementController>();

        followMovementAi = GetComponent<IFollowMovementAi>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bossStarted || !collision.CompareTag("Player"))
            return;

        if (followMovementAi != null)
            followMovementAi.FollowTarget(collision.gameObject);

        StartCoroutine(BossTestCoroutine());
    }

    private IEnumerator BossTestCoroutine()
    {
        bossStarted = true;

        animator.SetBool("IntroPlaying", true);
        yield return new WaitForSecondsRealtime(3);
        animator.SetBool("IntroPlaying", false);

        while (true)
        {
            yield return Punch();
            yield return new WaitForSecondsRealtime(3);
            gunController.Shoot();
            yield return new WaitForSecondsRealtime(3);
            movementController.SmoothMove(0, true, false);
            gunController.Shoot();
            yield return new WaitForSecondsRealtime(3);
        }
    }

    private IEnumerator Punch()
    {
        animator.SetBool("Punch", true);
        yield return new WaitForSecondsRealtime(punchPrepareTime);
        
        movementController.Dash();
        healthController.MakeInvincible(movementController.DashTime);
        contactDamageController.AlterDamage(5f, movementController.DashTime);
        yield return new WaitForSecondsRealtime(movementController.DashTime);

        animator.SetBool("Punch", false);
        yield return new WaitForSecondsRealtime(punchEndTime);
    }
}
