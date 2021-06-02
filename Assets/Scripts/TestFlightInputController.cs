using UnityEngine;

public class TestFlightInputController : MonoBehaviour
{
    private FlightMovementController movementController;

    private Vector2 move = Vector2.zero;

    private void Awake()
        => movementController = GetComponent<FlightMovementController>();

    private void Update()
        => move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

    private void FixedUpdate()
    {
        if (movementController == null || move == Vector2.zero)
            return;

        movementController.SmoothFly(move);
    }
}
