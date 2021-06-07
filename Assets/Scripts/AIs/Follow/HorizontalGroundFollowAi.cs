using System.Collections;
using UnityEngine;

public class HorizontalGroundFollowAi : FollowBaseAi, IFollowMovementAi
{
    private GroundMovementController movementController;

    protected GroundMovementController MovementController => movementController;

    private void Awake()
    {
        movementController = GetComponent<GroundMovementController>();
    }

    protected override IEnumerator LookAt(GameObject target)
    {
        if (movementController == null)
            yield break;

        while (IsLooking)
        {
            if (target == null)
                yield break;

            CheckShouldFlip(target);

            yield return new WaitForFixedUpdate();
        }
    }

    protected override IEnumerator Follow(GameObject target)
    {
        if (movementController == null)
            yield break;

        while (IsFollowing)
        {
            if (target == null)
                yield break;

            CheckShouldFlip(target);

            float direction = GetDirectionToFollowTarget(target);
            
            MovementController.SmoothMove(direction, false, false);

            yield return new WaitForFixedUpdate();
        }
    }

    protected float GetDirectionToFollowTarget(GameObject target)
    {
        float distanceToTarget = Mathf.Abs(target.transform.position.x - transform.position.x);
        double deccelerationDistance = 0.5 * Mathf.Pow(MovementController.CurrentVelocity, 2) / MovementController.Decceleration;
        float directionToTarget = Mathf.Sign(target.transform.position.x - transform.position.x);

        return distanceToTarget > DistanceToKeepFromTarget + deccelerationDistance ? directionToTarget : 0;
    }

    protected void CheckShouldFlip(GameObject target)
    {
        if ((target.transform.position.x > transform.position.x && !movementController.FacingRight)
            || (target.transform.position.x < transform.position.x && movementController.FacingRight))
            movementController.Flip();
    }
}
