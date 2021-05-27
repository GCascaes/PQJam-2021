using UnityEngine;

public class InputController : MonoBehaviour
{
    private MovementController movementController;
    private GunController gunController;
    private DefenseController defenseController;

    private float move = 0;
    private bool shouldJump = false;
    private bool holdJump = false;
    private bool shoot = false;
    private DefenseAction defenseAction = DefenseAction.None;

    private enum DefenseAction
    {
        None,
        Start,
        Stop,
    }

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
        gunController = GetComponent<GunController>();
        defenseController = GetComponent<DefenseController>();
    }

    private void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetButtonDown("Jump"))
            shouldJump = true;
        holdJump = Input.GetButton("Jump");
        
        shoot = Input.GetButton("Fire1");

        if (Input.GetButtonDown("Defend"))
            defenseAction = DefenseAction.Start;
        else if (Input.GetButtonUp("Defend"))
            defenseAction = DefenseAction.Stop;
    }

    private void FixedUpdate()
    {
        if (movementController != null && (move != 0 || shouldJump || holdJump))
            movementController.SmoothMove(move, shouldJump, holdJump);

        shouldJump = false;

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
