using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private bool airControl;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float jumpImpulse;
    [SerializeField] private int doubleJumps;
    [SerializeField] private LayerMask groundLayers;

    private int currentJumps = 0;
    private bool shouldJump = false;
    private bool isJumping = false;
    private bool facingRight = true;
    private float currentVelocity = 0;
    private Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!isJumping || airControl)
        {
            body.velocity = new Vector2(currentVelocity, body.velocity.y);

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
                body.AddForce(new Vector2(0, jumpImpulse), ForceMode2D.Impulse);
                isJumping = true;
                currentJumps++;
            }
            shouldJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.IsTouchingLayers(groundLayers))
        {
            isJumping = false;
            currentJumps = 0;
        }
    }

    public void SmoothMove(float direction, bool jump)
    {
        shouldJump = jump;

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
}