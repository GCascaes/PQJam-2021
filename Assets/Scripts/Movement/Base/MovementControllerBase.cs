using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementControllerBase : MonoBehaviour, IMovementController
{
    [SerializeField]
    private float maxVelocity;
    [SerializeField]
    private float maxShootingVelocity;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float decceleration;

    private bool movementEnabled = true;
    private bool facingRight;
    private float currentVelocity = 0;

    protected Rigidbody2D body;
    protected List<Collider2D> bodyColliders;
    protected Animator animator;
    private GunController gunController;

    public bool FacingRight => facingRight;
    public bool MovementEnabled => movementEnabled;
    public bool IsShooting => gunController != null && gunController.IsShooting;

    public float Acceleration => acceleration;
    public float Decceleration => decceleration;
    public float CurrentVelocity => currentVelocity;
    public float MaxVelocity =>
        gunController != null && gunController.IsShooting ? maxShootingVelocity : maxVelocity;

    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        bodyColliders = GetComponents<Collider2D>().Where(x => !x.isTrigger).ToList();
        animator = GetComponent<Animator>();
        gunController = GetComponent<GunController>();

        facingRight = transform.eulerAngles.y < 90 || transform.eulerAngles.y > 240;
    }

    public void DisableMovement() => movementEnabled = false;

    public void EnableMovement() => movementEnabled = true;

    public virtual void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(transform.up, 180);
    }

    public virtual void IncreaseVelocity(float percentage)
    {
        maxVelocity *= 1 + percentage / 100;
        maxShootingVelocity *= 1 + percentage / 100;
        acceleration *= 1 + percentage / 100;
        decceleration *= 1 + percentage / 100;
    }

    protected void UpdateCurrentVelocity(float direction)
    {
        float targetVelocity = direction * MaxVelocity;

        if (targetVelocity == currentVelocity)
        {
            // Maintain current velocity
        }
        else if ((direction > 0 && targetVelocity > currentVelocity && currentVelocity >= 0)
            || (direction < 0 && targetVelocity < currentVelocity && currentVelocity <= 0))
        {
            // Accelerate in direction of movement
            float newVelocity = currentVelocity + direction * Acceleration * Time.fixedDeltaTime;
            currentVelocity = Mathf.Sign(newVelocity) * Mathf.Min(Mathf.Abs(newVelocity), Mathf.Abs(targetVelocity));
        }
        else if ((direction > 0 && targetVelocity < currentVelocity)
            || (direction < 0 && targetVelocity > currentVelocity))
        {
            // Deccelerate in direction of movement
            float newVelocity = currentVelocity - direction * Decceleration * Time.fixedDeltaTime;

            var actualNewVelocity = currentVelocity >= 0
                ? Mathf.Max(0, newVelocity, targetVelocity)
                : Mathf.Min(0, newVelocity, targetVelocity);

            currentVelocity = actualNewVelocity;
        }
        else if (direction == 0 && currentVelocity != 0
            || (direction > 0 && targetVelocity > currentVelocity && currentVelocity < 0)
            || (direction < 0 && targetVelocity < currentVelocity && currentVelocity > 0))
        {
            // Deccelerate to zero
            float newVelocity = currentVelocity - Mathf.Sign(currentVelocity) * Mathf.Min(Decceleration * Time.fixedDeltaTime, Mathf.Abs(currentVelocity));

            var actualNewVelocity = currentVelocity >= 0
                ? Mathf.Max(0, newVelocity, targetVelocity)
                : Mathf.Min(0, newVelocity, targetVelocity);

            currentVelocity = actualNewVelocity;
        }
    }
}
