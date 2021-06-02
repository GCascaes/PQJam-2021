using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatrolBaseAi : MonoBehaviour, IPatrolMovementAi
{
    [SerializeField]
    private List<GameObject> patrolPoints;
    [SerializeField]
    private float PatrolGuardTime = 0;

    private bool isPatroling = false;
    private IEnumerator patrolCoroutine;

    protected virtual void Awake()
    {
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

    protected abstract IEnumerator MoveTo(Vector3 position);

    private IEnumerator Patrol()
    {
        while (isPatroling == true)
        {
            foreach (var point in patrolPoints)
            {
                yield return StartCoroutine(MoveTo(point.transform.position));
                yield return new WaitForSecondsRealtime(PatrolGuardTime);
            }
        }
    }
}
