using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [SerializeField]
    private float dropChancePercent;
    [SerializeField]
    private GameObject itemPrefab;

    private void Start()
    {
        if (TryGetComponent<HealthController>(out var healthController))
        {
            healthController.RegisterDeathAction(() =>
            {
                if (Random.Range(0, 100) <= dropChancePercent)
                    Instantiate(itemPrefab, transform.position, transform.rotation);
            });
        }
    }
}
