using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private MovementController movementController;
    [SerializeField]
    private GunController gunController;

    private float move = 0;
    private bool shouldJump = false;
    private bool holdJump = false;
    private bool shoot = false;

    private void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
            shouldJump = true;
        holdJump = Input.GetButton("Jump");
        shoot = Input.GetButton("Fire1");
    }

    private void FixedUpdate()
    {
        if (move != 0 || shouldJump || holdJump)
            movementController.SmoothMove(move, shouldJump, holdJump);

        shouldJump = false;

        if (shoot)
            gunController.Shoot();
    }
}
