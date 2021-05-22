using UnityEngine;

public class InputController : MonoBehaviour
{
    public MovementController movementController;

    private float move = 0;
    private bool shouldJump = false;

    private void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
            shouldJump = true;
    }

    private void FixedUpdate()
    {
        if (move != 0 || shouldJump)
            movementController.SmoothMove(move, shouldJump);

        shouldJump = false;
    }
}
