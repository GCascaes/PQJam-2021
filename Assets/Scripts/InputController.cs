using UnityEngine;

public class InputController : MonoBehaviour
{
    public MovementController movementController;

    private float move = 0;
    private bool shouldJump = false;
    private bool holdJump = false;

    private void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
            shouldJump = true;
        holdJump = Input.GetButton("Jump");
    }

    private void FixedUpdate()
    {
        if (move != 0 || shouldJump || holdJump)
            movementController.SmoothMove(move, shouldJump, holdJump);

        shouldJump = false;
    }
}
