using System.Collections;
using UnityEngine;

public class RunnerPatrolAi : MonoBehaviour, IPatrolMovementAi
{
    private bool isRunning = false;

    private IEnumerator runCoroutine;
    private GroundMovementController movementController;

    private void Awake()
    {
        movementController = GetComponent<GroundMovementController>();
        runCoroutine = Run();
    }

    public void StartPatroling()
    {
        isRunning = true;
        StopAllCoroutines();
        StartCoroutine(runCoroutine);
    }

    public void StopPatroling()
    {
        isRunning = false;
        StopAllCoroutines();
    }

    private IEnumerator Run()
    {
        if (movementController is null)
            yield break;

        while (isRunning == true)
        {
            movementController.SmoothMove(movementController.FacingRight ? 1 : -1, false, false);
            yield return new WaitForFixedUpdate();
        }
    }
}
