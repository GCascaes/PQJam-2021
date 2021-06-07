using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class ShotAttackBaseAi : GunController, IAttackAi
{
    [SerializeField]
    private AimingCapability aimingCapability = AimingCapability.None;

    private IMovementController movementController;
    private GameObject target;

    private Coroutine attackCoroutine;

    private bool isEngaged = false;
    protected bool IsEngaged => isEngaged;

    public enum AimingCapability
    {
        None,
        HorizontalFlip,
        FullCircleAim,
        BallisticAim,
    }

    protected override void Awake()
    {
        movementController = GetComponent<IMovementController>();
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        switch (aimingCapability)
        {
            case AimingCapability.HorizontalFlip:
                AimHorizontally();
                break;
            case AimingCapability.FullCircleAim:
                AimStraightShot();
                break;
            case AimingCapability.BallisticAim:
                AimHorizontally();
                AimBallistically();
                break;
        }

        base.FixedUpdate();
    }

    public void AttackTarget(GameObject target)
    {
        this.target = target;
        isEngaged = true;

        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);

        attackCoroutine = StartCoroutine(AttackRoutine());
    }

    public void StopAttacking()
    {
        isEngaged = false;
        shootContinuously = false;
        target = null;
        StopShooting();

        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);

        UpdateAnimatorShooting(false);
    }

    protected abstract IEnumerator AttackRoutine();

    protected void AimHorizontally()
    {
        if (target == null || movementController == null)
            return;

        if ((target.transform.position.x > transform.position.x && !movementController.FacingRight)
            || (target.transform.position.x < transform.position.x && movementController.FacingRight))
            movementController.Flip();
    }

    protected void AimStraightShot()
    {
        if (target == null)
            return;

        foreach (var shootPoint in shootPoints)
        {
            var angleToTarget = Vector2.SignedAngle(shootPoint.transform.right, target.transform.position - shootPoint.transform.position);
            shootPoint.transform.Rotate(shootPoint.transform.forward, angleToTarget);
        }
    }

    protected void AimBallistically()
    {
        if (target == null)
            return;

        if (!bulletPrefab.TryGetComponent<Rigidbody2D>(out var bulletBody))
            return;

        var shootPosition = shootPoints.Any() ? shootPoints.First().transform.position : transform.position;

        float g = bulletBody.gravityScale * Physics2D.gravity.y;
        float dx = Mathf.Abs(shootPosition.x - target.transform.position.x);
        float dy = shootPosition.y - target.transform.position.y;

        currentBulletVelocity = dx / Mathf.Sqrt(Mathf.Abs(2 * dy / g));
    }
}
