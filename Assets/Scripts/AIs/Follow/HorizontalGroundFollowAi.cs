using System.Collections;
using UnityEngine;

public class HorizontalGroundFollowAi : FollowBaseAi, IFollowMovementAi
{
    private GroundMovementController movementController;

    private void Awake()
    {
        movementController = GetComponent<GroundMovementController>();
    }

    protected override IEnumerator LookAt(GameObject target)
    {
        if (movementController is null)
            yield break;

        while (IsLooking)
        {
            if ((target.transform.position.x > transform.position.x && !movementController.FacingRight)
                || (target.transform.position.x < transform.position.x && movementController.FacingRight))
                movementController.Flip();

            yield return new WaitForFixedUpdate();
        }
    }

    protected override IEnumerator Follow(GameObject target)
    {
        if (movementController is null)
            yield break;

        while (IsFollowing)
        {
            if ((target.transform.position.x > transform.position.x && !movementController.FacingRight)
                || (target.transform.position.x < transform.position.x && movementController.FacingRight))
                movementController.Flip();

            while (Mathf.Abs(target.transform.position.x - transform.position.x)
                > DistanceToKeepFromTarget + 0.5*Mathf.Pow(movementController.CurrentVelocity, 2)/movementController.Decceleration)
            {
                movementController.SmoothMove(Mathf.Sign(target.transform.position.x - transform.position.x), false, false);
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
