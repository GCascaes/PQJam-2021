using UnityEngine;

public class ItemsController : MonoBehaviour
{
    private GunPowerUpController gunController;
    private HealthController healthController;

    private void Awake()
    {
        gunController = GetComponent<GunPowerUpController>();
        healthController = GetComponent<HealthController>();
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
                if (gunController != null)
                    gunController.ActivateSpreadShot();
                break;
            case CollectibleType.SuperShot:
                if (gunController != null)
                    gunController.ActivateSuperShot();
                break;
            case CollectibleType.QuickShot:
                if (gunController != null)
                    gunController.ActivateQuickShot();
                break;
        }

        Destroy(collision.gameObject);
    }
}
