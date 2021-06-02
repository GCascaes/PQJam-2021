﻿using System.Collections;
using UnityEngine;

public class HorizontalGroundPatrolAi : PatrolBaseAi, IPatrolMovementAi
{
    private GroundMovementController movementController;

    protected override void Awake()
    {
        movementController = GetComponent<GroundMovementController>();
        base.Awake();
    }

    protected override IEnumerator MoveTo(Vector3 position)
    {
        if (movementController is null)
            yield break;

        while (Mathf.Abs(position.x - transform.position.x)
            > 0.5 * Mathf.Pow(movementController.CurrentVelocity, 2) / movementController.Decceleration)
        {
            movementController.SmoothMove(Mathf.Sign(position.x - transform.position.x), false, false);
            yield return new WaitForFixedUpdate();
        }
    }
}
