using UnityEngine;

public class InputController : MonoBehaviour
{
    private GunController gunController;
    private PowerUpController powerUpController;
    private DefenseController defenseController;
    private GroundMovementController movementController;

    private float move = 0;
    private bool shouldJump = false;
    private bool holdJump = false;
    private bool shoot = false;
    private bool activatePowerUp = false;
    private DefenseAction defenseAction = DefenseAction.None;

    private enum DefenseAction
    {
        None,
        Start,
        Stop,
    }

    private void Awake()
    {
        gunController = GetComponent<GunController>();
        powerUpController = GetComponent<PowerUpController>();
        defenseController = GetComponent<DefenseController>();
        movementController = GetComponent<GroundMovementController>();
    }

    private void Update()
    {
        move = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
            shouldJump = true;
        holdJump = Input.GetButton("Jump");

        shoot = Input.GetButton("Fire");

        if (Input.GetButtonDown("Defend"))
            defenseAction = DefenseAction.Start;
        else if (Input.GetButtonUp("Defend"))
            defenseAction = DefenseAction.Stop;

        if (Input.GetButtonDown("PowerUp"))
            activatePowerUp = true;
    }

    private void FixedUpdate()
    {
        if (movementController != null && (move != 0 || shouldJump || holdJump))
            movementController.SmoothMove(move, shouldJump, holdJump);

        shouldJump = false;

        if (powerUpController != null && activatePowerUp)
            powerUpController.ActivatePowerUp();

        activatePowerUp = false;

        if (gunController != null && shoot)
            gunController.Shoot();

        switch (defenseAction)
        {
            case DefenseAction.Start when defenseController != null:
                defenseController.StartDefending();
                break;
            case DefenseAction.Stop when defenseController != null:
                defenseController.StopDefending();
                break;
        }
        defenseAction = DefenseAction.None;
    }
}
