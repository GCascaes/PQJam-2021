using System.Linq;
using UnityEngine;

public class GroundMovementController : MovementControllerBase, IMovementController
{
    [SerializeField]
    private bool airControl;
    [SerializeField]
    private float baseJumpHeight;
    [SerializeField]
    private float holdJumpHeightMultiplier;
    [SerializeField]
    [Range(0, 1)]
    private float holdJumpFallAttenuation;
    [SerializeField]
    private float doubleJumpVelocityModifier;
    [SerializeField]
    private int doubleJumps;
    [SerializeField]
    private LayerMask groundLayers;

    private int currentJumps = 0;
    private bool shouldJump = false;
    private bool isJumping = false;
    private bool holdJump = false;
    private float baseJumpVelocity;
    private float holdJumpAcceleration;
    private float holdJumpFallAcceleration;

    private float movementDirection = 0;

    protected override void Awake()
    {
        base.Awake();

        baseJumpVelocity = Mathf.Sqrt(2 * (-Physics2D.gravity.y) * body.gravityScale * baseJumpHeight / 1);
        holdJumpAcceleration = (-Physics2D.gravity.y) * body.gravityScale - Mathf.Pow(baseJumpVelocity, 2) / (2 * holdJumpHeightMultiplier * baseJumpHeight);
        holdJumpFallAcceleration = holdJumpFallAttenuation * (-Physics2D.gravity.y) * body.gravityScale;
    }

    private void FixedUpdate()
    {
        HandleMoving();
        HandleJumping();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.otherCollider.IsTouchingLayers(groundLayers))
            return;

        SetJumping(false);
        currentJumps = 0;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (bodyColliders.Any(x => x.IsTouchingLayers(groundLayers)))
            return;

        SetJumping(true);
        currentJumps = 1;
    }

    public void SmoothMove(float direction, bool jump, bool holdJump)
    {
        if (!MovementEnabled)
            return;

        shouldJump = jump;
        this.holdJump = holdJump;

        movementDirection = direction;
    }

    private void HandleMoving()
    {
        if (isJumping && !airControl)
            return;

        float direction = movementDirection != 0 ? Mathf.Sign(movementDirection) : 0;

        UpdateCurrentVelocity(direction);

        body.velocity = new Vector2(CurrentVelocity, body.velocity.y);

        if (animator != null)
            animator.SetFloat("Speed", Mathf.Abs(CurrentVelocity));

        if (!IsShooting &&
            ((CurrentVelocity > 0.1 && !FacingRight) || (CurrentVelocity < -0.1 && FacingRight)))
            Flip();

        movementDirection = 0;
    }

    private void HandleJumping()
    {
        if (shouldJump)
            Jump();
        else if (holdJump)
            HoldJump();
    }

    private void Jump()
    {
        shouldJump = false;
        
        if (currentJumps > doubleJumps)
            return;

        float jumpVelocity = baseJumpVelocity;
        if (currentJumps > 0)
            jumpVelocity *= doubleJumpVelocityModifier;

        body.velocity = new Vector2(body.velocity.x, jumpVelocity);

        SetJumping(true);
        currentJumps++;
    }

    private void HoldJump()
    {
        var yAcceleration = body.velocity.y > 0 ? holdJumpAcceleration : holdJumpFallAcceleration;
        body.velocity = new Vector2(body.velocity.x, body.velocity.y + yAcceleration * Time.fixedDeltaTime);
        holdJump = false;
    }

    private void SetJumping(bool jumping)
    {
        isJumping = jumping;
        if (animator != null)
            animator.SetBool("InAir", jumping);
    }
}
