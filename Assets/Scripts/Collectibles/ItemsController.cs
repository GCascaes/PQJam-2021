using UnityEngine;

public class ItemsController : MonoBehaviour
{
    private GunController gunController;
    private HealthController healthController;
    private PowerUpController powerUpController;

    private void Awake()
    {
        gunController = GetComponent<GunController>();
        healthController = GetComponent<HealthController>();
        powerUpController = GetComponent<PowerUpController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out Collectible collectible))
            return;

        switch (collectible.Type)
        {
            case CollectibleType.Gun:
                if (gunController != null)
                    gunController.EquipGun();
                break;
            case CollectibleType.Heart:
                if (healthController != null)
                    healthController.RefillHealth(10 * collectible.Quantity);
                break;
            case CollectibleType.SuperHeart:
                GameManager.instance.AddMaxHealth(10 * collectible.Quantity);
                break;
            case CollectibleType.SpreadShot:
                if (powerUpController != null)
                    powerUpController.CollectPowerUp(PowerUpType.SpreadShot);
                break;
            case CollectibleType.SuperShot:
                if (powerUpController != null)
                    powerUpController.CollectPowerUp(PowerUpType.SuperShot);
                break;
            case CollectibleType.QuickShot:
                if (powerUpController != null)
                    powerUpController.CollectPowerUp(PowerUpType.QuickShot);
                break;
        }

        Destroy(collision.gameObject);
    }
}
