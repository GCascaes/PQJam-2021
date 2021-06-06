using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    private static PowerUpType currentlyHeldPowerUp = PowerUpType.None;

    private GunPowerUpController gunPowerUpController;

    private void Awake()
    {
        gunPowerUpController = GetComponent<GunPowerUpController>();
    }

    public void CollectPowerUp(PowerUpType powerUpType)
    {
        currentlyHeldPowerUp = powerUpType;

    }

    public void ActivatePowerUp()
    {
        if (gunPowerUpController is null)
            return;

        switch (currentlyHeldPowerUp)
        {
            case PowerUpType.SpreadShot:
                gunPowerUpController.ActivateSpreadShot();
                break;
            case PowerUpType.SuperShot:
                gunPowerUpController.ActivateSuperShot();
                break;
            case PowerUpType.QuickShot:
                gunPowerUpController.ActivateQuickShot();
                break;
        }

        currentlyHeldPowerUp = PowerUpType.None;
    }
}
