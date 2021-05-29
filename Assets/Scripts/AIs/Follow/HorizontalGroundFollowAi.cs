using System.Collections;
using UnityEngine;

public class HorizontalGroundFollowAi : MonoBehaviour, IFollowMovementAi
{
    [SerializeField]
    private float distanceToKeepFromTarget = 0;

    private bool isLooking = false;
    private bool isFollowing = false;

    private GroundMovementController movementController;

    private void Awake()
    {
        movementController = GetComponent<GroundMovementController>();
    }

    public void LookAtTarget(GameObject newTarget)
    {
        if (isFollowing)
            return;

        isLooking = true;
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

    private IEnumerator LookAt(GameObject target)
    {
        if (movementController is null)
            yield break;

        while (isLooking)
        {
            if ((target.transform.position.x > transform.position.x && !movementController.FacingRight)
                || (target.transform.position.x < transform.position.x && movementController.FacingRight))
                movementController.Flip();

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator Follow(GameObject target)
    {
        if (movementController is null)
            yield break;

        while (isFollowing)
        {
            while (Mathf.Abs(target.transform.position.x - transform.position.x)
                > distanceToKeepFromTarget + 0.5*Mathf.Pow(movementController.CurrentVelocity, 2)/movementController.Decceleration)
            {
                movementController.SmoothMove(Mathf.Sign(target.transform.position.x - transform.position.x), false, false);
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
