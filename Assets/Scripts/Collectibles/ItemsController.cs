using UnityEngine;

public class ItemsController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out Collectible collectible))
            return;

        switch (collectible.Type)
        {
            case CollectibleType.Gun:
                CollectGun();
                break;
        }

        Destroy(collision.gameObject);
    }

    private void CollectGun()
    {
        if (!TryGetComponent(out GunController gunController))
            return;

        gunController.EquipGun();
    }
}
