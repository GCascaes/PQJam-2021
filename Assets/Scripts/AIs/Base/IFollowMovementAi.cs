using UnityEngine;

public interface IFollowMovementAi
{
    void LookAtTarget(GameObject newTarget);
    void FollowTarget(GameObject newTarget);
    void StopFollowing();
}
