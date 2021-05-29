using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalGroundPatrolAi : MonoBehaviour, IPatrolMovementAi
{
    [SerializeField]
    private List<GameObject> patrolPoints;
    [SerializeField]
    private float PatrolGuardTime = 0;

    private bool isPatroling = false;
    private IEnumerator patrolCoroutine;

    private GroundMovementController movementController;

    private void Awake()
    {
        movementController = GetComponent<GroundMovementController>();
        patrolCoroutine = Patrol();
    }

    public void StartPatroling()
    {
        isPatroling = true;
        StopAllCoroutines();
        StartCoroutine(patrolCoroutine);
    }

    public void StopPatroling()
    {
        isPatroling = false;
        StopAllCoroutines();
    }

    private IEnumerator Patrol()
    {
        while (isPatroling == true)
        {
            foreach(var point in patrolPoints)
            {
                yield return StartCoroutine(MoveTo(point));
                yield return new WaitForSecondsRealtime(PatrolGuardTime);
            }
        }
    }

    private IEnumerator MoveTo(GameObject position)
    {
        if (movementController is null)
            yield break;

        while (Mathf.Abs(position.transform.position.x - transform.position.x)
            > 0.5*Mathf.Pow(movementController.CurrentVelocity, 2)/movementController.Decceleration)
         {
            movementController.SmoothMove(Mathf.Sign(position.transform.position.x - transform.position.x), false, false);
            yield return new WaitForFixedUpdate();
        }
    }
}
