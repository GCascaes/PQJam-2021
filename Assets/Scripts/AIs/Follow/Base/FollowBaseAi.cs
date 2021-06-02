using System.Collections;
using UnityEngine;

public abstract class FollowBaseAi : MonoBehaviour, IFollowMovementAi
{
    [SerializeField]
    private float distanceToKeepFromTarget = 0;

    private bool isLooking = false;
    private bool isFollowing = false;

    public bool IsLooking => isLooking;
    public bool IsFollowing => isFollowing;
    public float DistanceToKeepFromTarget => distanceToKeepFromTarget;

    public void LookAtTarget(GameObject newTarget)
    {
        if (isFollowing)
            return;

        isLooking = true;
        isFollowing = false;
        StopAllCoroutines();
        StartCoroutine(LookAt(newTarget));
    }

    public void FollowTarget(GameObject target)
    {
        isLooking = false;
        isFollowing = true;
        StopAllCoroutines();
        StartCoroutine(Follow(target));
    }

    public void StopFollowing()
    {
        isLooking = false;
        isFollowing = false;
        StopAllCoroutines();
    }

    protected abstract IEnumerator LookAt(GameObject target);

    protected abstract IEnumerator Follow(GameObject target);
}
