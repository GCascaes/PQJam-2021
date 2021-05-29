using System.Linq;
using UnityEngine;

public class GroundMovementController : MonoBehaviour, IMovementController
{
    [SerializeField]
    private bool airControl;
    [SerializeField]
    private float maxVelocity;
    [SerializeField]
    private float maxShootingVelocity;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float decceleration;
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

    private bool movementEnabled = true;

    private bool facingRight;
    private float movementDirection = 0;
    private float currentVelocity = 0;

    private Rigidbody2D body;
    private Collider2D[] colliders;
    private Animator animator;
    private GunController gunController;

    public bool FacingRight => facingRight;

    public float CurrentVelocity => currentVelocity;
    public float Decceleration => decceleration;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        colliders = GetComponents<Collider2D>();
        animator = GetComponent<Animator>();
        gunController = GetComponent<GunController>();

        facingRight = transform.eulerAngles.y < 90 || transform.eulerAngles.y > 240;

        baseJumpVelocity = Mathf.Sqrt(2 * (-Physics2D.gravity.y) * body.gravityScale * baseJumpHeight / 1);
        holdJumpAcceleration = (-Physics2D.gravity.y) * body.gravityScale - Mathf.Pow(baseJumpVelocity, 2) / (2 * holdJumpHeightMultiplier * baseJumpHeight);
        holdJumpFallAcceleration = holdJumpFallAttenuation * (-Physics2D.gravity.y) * body.gravityScale;
    }

    private void FixedUpdate()
    {
        if (!isJumping || airControl)
        {
            float direction = movementDirection != 0 ? Mathf.Sign(movementDirection) : 0;

            float targetVelocity = direction * maxVelocity;
            if (gunController != null && gunController.IsShooting)
                targetVelocity = direction * maxShootingVelocity;

            if (targetVelocity == currentVelocity)
            {
                // Maintain current velocity
            }
            else if ((direction > 0 && targetVelocity > currentVelocity)
                ||   (direction < 0 && targetVelocity < currentVelocity))
            {
                // Accelerate in direction of movement
                float newVelocity = currentVelocity + direction * acceleration * Time.fixedDeltaTime;
                currentVelocity = Mathf.Sign(newVelocity) * Mathf.Min(Mathf.Abs(newVelocity), Mathf.Abs(targetVelocity));
            }
            else if ((direction > 0 && targetVelocity < currentVelocity)
                ||   (direction < 0 && targetVelocity > currentVelocity))
            {
                // Deccelerate in direction of movement
                float newVelocity = currentVelocity - direction * decceleration * Time.fixedDeltaTime;
                currentVelocity = Mathf.Sign(newVelocity) * Mathf.Max(Mathf.Abs(newVelocity), Mathf.Abs(targetVelocity));
            }
            else if (direction == 0 && currentVelocity != 0)
            {
                // Deccelerate to zero
                float newVelocity = currentVelocity - Mathf.Sign(currentVelocity) * Mathf.Min(decceleration * Time.fixedDeltaTime, Mathf.Abs(currentVelocity));
                currentVelocity = Mathf.Sign(newVelocity) * Mathf.Max(Mathf.Abs(newVelocity), Mathf.Abs(targetVelocity));
            }

            body.velocity = new Vector2(currentVelocity, body.velocity.y);

            if (animator != null)
                animator.SetFloat("Speed", Mathf.Abs(currentVelocity));

            if ((gunController is null || !gunController.IsShooting) &&
                ((currentVelocity > 0.1 && !facingRight) || (currentVelocity < -0.1 && facingRight)))
                Flip();

            movementDirection = 0;
        }

        if (shouldJump)
        {
            if (currentJumps <= doubleJumps)
            {
                float jumpVelocity = baseJumpVelocity;
                if (currentJumps > 0)
                    jumpVelocity *= doubleJumpVelocityModifier;

                body.velocity = new Vector2(body.velocity.x, jumpVelocity);

                SetJumping(true);
                currentJumps++;
            }
            shouldJump = false;
        }
        else if (holdJump)
        {
            var yAcceleration = body.velocity.y > 0 ? holdJumpAcceleration : holdJumpFallAcceleration;
            body.velocity = new Vector2(body.velocity.x, body.velocity.y + yAcceleration * Time.fixedDeltaTime);
            holdJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.IsTouchingLayers(groundLayers))
        {
            SetJumping(false);
            currentJumps = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!colliders.Any(x => x.IsTouchingLayers(groundLayers)))
        {
            SetJumping(true);
            currentJumps = 1;
        }
    }

    public void DisableMovement() => movementEnabled = false;
    
    public void EnableMovement() => movementEnabled = true;
    
    public void SmoothMove(float direction, bool jump, bool holdJump)
    {
        if (!movementEnabled)
            return;

        shouldJump = jump;
        this.holdJump = holdJump;

        movementDirection = direction;
    }

    public void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(transform.up, 180);
    }

    private void SetJumping(bool jumping)
    {
        isJumping = jumping;
        if (animator != null)
            animator.SetBool("InAir", jumping);
    }
}
