using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    private static PowerUpType currentlyHeldPowerUp = PowerUpType.None;

    private GunPowerUpController gunPowerUpController;

    private void Start()
    {
        gunPowerUpController = GetComponent<GunPowerUpController>();
        PlayerUI.instance.SetPowerUpUI(currentlyHeldPowerUp);
    }

    public void CollectPowerUp(PowerUpType powerUpType)
    {
        currentlyHeldPowerUp = powerUpType;
        PlayerUI.instance.SetPowerUpUI(powerUpType);
    }

    public void ActivatePowerUp()
    {
        if (gunPowerUpController == null)
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
        PlayerUI.instance.SetPowerUpUI(currentlyHeldPowerUp);
    }
}
