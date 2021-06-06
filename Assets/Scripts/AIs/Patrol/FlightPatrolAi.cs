using System.Collections;
using UnityEngine;

public class FlightPatrolAi : PatrolBaseAi, IPatrolMovementAi
{
    private FlightMovementController movementController;

    private void Awake()
    {
        movementController = GetComponent<FlightMovementController>();
    }

    protected override IEnumerator MoveTo(Vector3 position)
    {
        if (movementController is null)
            yield break;

        while (Vector2.Distance(position, transform.position)
            > 0.5 * Mathf.Pow(movementController.CurrentVelocity, 2) / movementController.Decceleration)
        {
            movementController.SmoothFly(position - transform.position);
            yield return new WaitForFixedUpdate();
        }
    }
}
