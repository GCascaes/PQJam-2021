using System.Collections;
using UnityEngine;

public class HorizontalShotAttackAi : GunController, IAttackAi
{
    [SerializeField]
    private float firstShootDelay;
    [SerializeField]
    private bool shootsContinuously = true;
    [SerializeField]
    private bool flipsToAim;

    private IMovementController movementController;
    
    private GameObject target;

    protected override void Awake()
    {
        movementController = GetComponent<IMovementController>();
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        if (flipsToAim
            && target != null
            && movementController != null)
        {
            if ((target.transform.position.x > transform.position.x && !movementController.FacingRight)
                || (target.transform.position.x < transform.position.x && movementController.FacingRight))
                movementController.Flip();
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
