using System.Collections;
using System.Linq;
using UnityEngine;

public class ShotAttackAi : GunController, IAttackAi
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

    private void AimHorizontally()
    {
        if (target is null || movementController is null)
            return;

        if ((target.transform.position.x > transform.position.x && !movementController.FacingRight)
            || (target.transform.position.x < transform.position.x && movementController.FacingRight))
            movementController.Flip();
    }

    private void AimStraightShot()
    {
        if (target is null)
            return;

        foreach (var shootPoint in shootPoints)
        {
            var angleToTarget = Vector2.SignedAngle(shootPoint.transform.right, target.transform.position - shootPoint.transform.position);
            shootPoint.transform.Rotate(shootPoint.transform.forward, angleToTarget);
        }
    }

    private void AimBallistically()
    {
        if (target is null)
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
