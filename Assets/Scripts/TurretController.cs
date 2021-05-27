using UnityEngine;

public class TurretController : GunController
{
    private void OnBecameVisible()
    {
        shootContinuously = true;
        Shoot();
    }

    private void OnBecameInvisible()
    {
        shootContinuously = false;
    }
}
