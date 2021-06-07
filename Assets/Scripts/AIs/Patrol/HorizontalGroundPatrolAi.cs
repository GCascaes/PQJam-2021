using System.Collections;
using UnityEngine;

public class HorizontalGroundPatrolAi : PatrolBaseAi, IPatrolMovementAi
{
    private GroundMovementController movementController;

    private void Awake()
    {
        movementController = GetComponent<GroundMovementController>();
    }

    protected override IEnumerator MoveTo(Vector3 position)
    {
        if (movementController == null)
            yield break;

        while (Mathf.Abs(position.x - transform.position.x)
            > 0.5 * Mathf.Pow(movementController.CurrentVelocity, 2) / movementController.Decceleration)
        {
            movementController.SmoothMove(Mathf.Sign(position.x - transform.position.x), false, false);
            yield return new WaitForFixedUpdate();
        }
    }
}
