using System.Collections;
using UnityEngine;

public class FlightFollowAi : FollowBaseAi, IFollowMovementAi
{
    private FlightMovementController movementController;

    private void Awake()
    {
        movementController = GetComponent<FlightMovementController>();
    }

    protected override IEnumerator LookAt(GameObject target)
    {
        if (movementController == null)
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
        if (movementController == null)
            yield break;

        while (IsFollowing)
        {
            while (Vector2.Distance(target.transform.position, transform.position)
                > DistanceToKeepFromTarget + 0.5 * Mathf.Pow(movementController.CurrentVelocity, 2) / movementController.Decceleration)
            {
                movementController.SmoothFly(target.transform.position - transform.position);
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
