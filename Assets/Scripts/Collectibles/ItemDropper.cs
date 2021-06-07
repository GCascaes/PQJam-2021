using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [SerializeField]
    private float dropChancePercent;
    [SerializeField]
    private List<GameObject> itemsPrefab;

    private void Start()
    {
        if (TryGetComponent<HealthController>(out var healthController)
            && itemsPrefab != null
            && itemsPrefab.Any())
        {
            healthController.RegisterDeathAction(() =>
            {
                if (Random.Range(0, 100) <= dropChancePercent)
                {
                    int itemIndex = Random.Range(0, itemsPrefab.Count);
                    var itemPrefab = itemsPrefab[itemIndex];
                    Instantiate(itemPrefab, transform.position, Quaternion.identity);
                }
            });
        }
    }
}
