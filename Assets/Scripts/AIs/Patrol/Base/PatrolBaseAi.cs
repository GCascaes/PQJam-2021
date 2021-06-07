using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PatrolBaseAi : MonoBehaviour, IPatrolMovementAi
{
    [SerializeField]
    private List<GameObject> patrolPoints;
    [SerializeField]
    private float PatrolGuardTime = 0;

    private bool isPatroling = false;

    public void StartPatroling()
    {
        isPatroling = true;
        StopAllCoroutines();
        StartCoroutine(Patrol());
    }

    public void StopPatroling()
    {
        isPatroling = false;
        StopAllCoroutines();
    }

    protected abstract IEnumerator MoveTo(Vector3 position);

    private IEnumerator Patrol()
    {
        if (patrolPoints == null || !patrolPoints.Any())
            yield break;

        while (isPatroling == true)
        {
            foreach (var point in patrolPoints)
            {
                if (point == null)
                {
                    yield return new WaitForFixedUpdate();
                    continue;
                }

                yield return MoveTo(point.transform.position);
                yield return new WaitForSecondsRealtime(PatrolGuardTime);
            }
        }
    }
}
