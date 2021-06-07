using System.Collections;
using UnityEngine;

public class GroundBossFollowAi : HorizontalGroundFollowAi, IFollowMovementAi
{
    [SerializeField]
    private float bulletHeigthToAvoid;
    [SerializeField]
    private float jumpCooldown;

    private bool shouldJump = false;
    private float lastJumpTime = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return;

        if (collision.TryGetComponent<Rigidbody2D>(out var body)
            && Mathf.Abs(body.position.y - transform.position.y) <= bulletHeigthToAvoid
            && Time.realtimeSinceStartup - lastJumpTime > jumpCooldown
            && !MovementController.IsDashing)
        {
            if ((body.position.x < transform.position.x && body.velocity.x > 0)
                || (body.position.x > transform.position.x && body.velocity.x < 0))
            {
                lastJumpTime = Time.realtimeSinceStartup;
                shouldJump = true;
            }
        }
    }

    protected override IEnumerator Follow(GameObject target)
    {
        if (MovementController == null)
            yield break;

        while (IsFollowing)
        {
            if (target == null)
                yield break;

            CheckShouldFlip(target);

            float direction = GetDirectionToFollowTarget(target);

            MovementController.SmoothMove(direction, shouldJump, false);

            shouldJump = false;

            yield return new WaitForFixedUpdate();
        }
    }
}
