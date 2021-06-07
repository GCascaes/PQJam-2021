using Assets.Scripts.Extensions;
using UnityEngine;

public class FlightMovementController : MovementControllerBase, IMovementController
{
    [SerializeField]
    private float minSteer = 360;

    private Vector2 currentDirection;
    private Vector2 desiredDirection = Vector2.zero;

    protected override void Awake()
    {
        base.Awake();
        currentDirection = FacingRight ? Vector2.right : Vector2.left;
    }

    private void FixedUpdate() => HandleFlight();

    public void SmoothFly(Vector2 direction)
    {
        if (!MovementEnabled)
            return;

        desiredDirection = direction.normalized;
    }

    private void HandleFlight()
    {
        Steer();
        ControlFlightVelocity();
        UpdateBodyVelocity();

        desiredDirection = Vector2.zero;
    }

    private void Steer()
    {
        if (desiredDirection == Vector2.zero)
            return;

        if (Mathf.Abs(Vector2.SignedAngle(currentDirection, desiredDirection)) > 90
            && CurrentVelocity > 0.1)
            return;

        float desiredSteer = Vector2.SignedAngle(currentDirection, desiredDirection);
        float maxSteer = Mathf.Min(360, minSteer * MaxVelocity / Mathf.Max(0.01f, CurrentVelocity)) * Time.fixedDeltaTime;
        float actualSteer = Mathf.Sign(desiredSteer) * Mathf.Min(Mathf.Abs(desiredSteer), maxSteer);

        currentDirection.Rotate(actualSteer);
    }

    private void ControlFlightVelocity()
    {
        float direction = 0;

        if (desiredDirection != Vector2.zero
            && Mathf.Abs(Vector2.SignedAngle(currentDirection, desiredDirection)) <= 90)
            direction = 1;
        
        UpdateCurrentVelocity(direction);
    }

    private void UpdateBodyVelocity()
    {
        if ((currentDirection.x > 0 && !FacingRight)
            || (currentDirection.x < 0 && FacingRight))
            Flip();

        body.velocity = currentDirection.normalized * CurrentVelocity;
    }
}
