using UnityEngine;

public class ItemsController : MonoBehaviour
{
    private GunController gunController;
    private HealthController healthController;

    private void Awake()
    {
        gunController = GetComponent<GunController>();
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
                if (healthController != null)
                    healthController.SuperRefillHealth(10 * collectible.Quantity);
                break;
        }

        Destroy(collision.gameObject);
    }
}
