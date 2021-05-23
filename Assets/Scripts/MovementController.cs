using System.Linq;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private bool airControl;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float baseJumpHeight;
    [SerializeField] private float holdJumpHeightMultiplier;
    [SerializeField] private float doubleJumpVelocityModifier;
    [SerializeField] private int doubleJumps;
    [SerializeField] private LayerMask groundLayers;

    private int currentJumps = 0;
    private bool shouldJump = false;
    private bool isJumping = false;
    private bool holdJump = false;
    private bool facingRight = true;
    private float currentVelocity = 0;
    private float baseJumpVelocity;
    private float holdJumpAcceleration;
    private Rigidbody2D body;
    private Collider2D[] colliders;
    private Animator animator;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        colliders = GetComponents<Collider2D>();
        animator = GetComponent<Animator>();

        baseJumpVelocity = Mathf.Sqrt(2 * (-Physics2D.gravity.y) * body.gravityScale * baseJumpHeight / 1);
        holdJumpAcceleration = (-Physics2D.gravity.y) * body.gravityScale - Mathf.Pow(baseJumpVelocity, 2) / (2 * holdJumpHeightMultiplier * baseJumpHeight);
    }

    private void FixedUpdate()
    {
        if (!isJumping || airControl)
        {
            body.velocity = new Vector2(currentVelocity, body.velocity.y);
            animator.SetFloat("Speed", Mathf.Abs(currentVelocity));

            if ((currentVelocity > 0 && !facingRight) || (currentVelocity < 0 && facingRight))
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
            body.velocity = new Vector2(body.velocity.x, body.velocity.y + holdJumpAcceleration * Time.fixedDeltaTime);
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

    public void SmoothMove(float direction, bool jump, bool holdJump)
    {
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
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void SetJumping(bool jumping)
    {
        isJumping = jumping;
        animator.SetBool("InAir", jumping);
    }
}
