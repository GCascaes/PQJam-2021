using System.Linq;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField]
    private bool airControl;
    [SerializeField]
    private float maxVelocity;
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

    private bool facingRight = true;
    private float currentVelocity = 0;

    private Rigidbody2D body;
    private Collider2D[] colliders;
    private Animator animator;
    private GunController gunController;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        colliders = GetComponents<Collider2D>();
        animator = GetComponent<Animator>();
        gunController = GetComponent<GunController>();

        baseJumpVelocity = Mathf.Sqrt(2 * (-Physics2D.gravity.y) * body.gravityScale * baseJumpHeight / 1);
        holdJumpAcceleration = (-Physics2D.gravity.y) * body.gravityScale - Mathf.Pow(baseJumpVelocity, 2) / (2 * holdJumpHeightMultiplier * baseJumpHeight);
        holdJumpFallAcceleration = holdJumpFallAttenuation * (-Physics2D.gravity.y) * body.gravityScale;
    }

    private void FixedUpdate()
    {
        if (!isJumping || airControl)
        {
            // TODO Reduce maximum speed when moving and shooting
            body.velocity = new Vector2(currentVelocity, body.velocity.y);
            animator.SetFloat("Speed", Mathf.Abs(currentVelocity));

            if ((gunController is null || !gunController.IsShooting) &&
                ((currentVelocity > 0 && !facingRight) || (currentVelocity < 0 && facingRight)))
                Flip();

            if (Mathf.Abs(currentVelocity) > 0)
            {
                currentVelocity -= Mathf.Sign(currentVelocity) * Mathf.Min(decceleration, Mathf.Abs(currentVelocity));
            }
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

        if (direction == 0)
            return;

        if (!isJumping || airControl)
        {
            float newVelocity = currentVelocity + Mathf.Sign(direction) * acceleration;
            currentVelocity = Mathf.Min(Mathf.Abs(newVelocity), maxVelocity) * Mathf.Sign(newVelocity);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(transform.up, 180);
    }

    private void SetJumping(bool jumping)
    {
        isJumping = jumping;
        animator.SetBool("InAir", jumping);
    }
}
