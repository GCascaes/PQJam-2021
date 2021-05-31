using System.Collections;
using System.Linq;
using UnityEngine;

public class StraightShotAttackAi : GunController, IAttackAi
{
    [SerializeField]
    private float firstShootDelay;
    [SerializeField]
    private bool shootsContinuously = true;
    [SerializeField]
    private AimingCapability aimingCapability = AimingCapability.None;

    private IMovementController movementController;
    private GameObject target;

    public enum AimingCapability
    {
        None,
        HorizontalFlip,
        FullCircleAim,
    }

    protected override void Awake()
    {
        movementController = GetComponent<IMovementController>();
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        if (aimingCapability == AimingCapability.HorizontalFlip
            && target != null
            && movementController != null)
        {
            if ((target.transform.position.x > transform.position.x && !movementController.FacingRight)
                || (target.transform.position.x < transform.position.x && movementController.FacingRight))
                movementController.Flip();
        }

        if (aimingCapability == AimingCapability.FullCircleAim
            && target != null)
        {
            foreach(var shootPoint in shootPoints)
            {
                var angleToTarget = Vector2.SignedAngle(shootPoint.transform.right, target.transform.position - shootPoint.transform.position);
                shootPoint.transform.Rotate(shootPoint.transform.forward, angleToTarget);
            }
        }

        base.FixedUpdate();
    }

    public void AttackTarget(GameObject target)
    {
        this.target = target;
        StopAllCoroutines();
        StartCoroutine(AttackRoutine());
    }

    public void StopAttacking()
    {
        shootContinuously = false;
        target = null;
        StopAllCoroutines();
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSecondsRealtime(firstShootDelay);
        shootContinuously = shootsContinuously;
        Shoot();
    }
}
