using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [SerializeField]
    private float dropChancePercent;
    [SerializeField]
    private List<GameObject> itemsPrefab;

    private void Start()
    {
        if (TryGetComponent<HealthController>(out var healthController))
        {
            healthController.RegisterDeathAction(() =>
            {
                if (Random.Range(0, 100) <= dropChancePercent)
                {
                    int itemIndex = Random.Range(0, itemsPrefab.Count);
                    var itemPrefab = itemsPrefab[itemIndex];
                    Instantiate(itemPrefab, transform.position, transform.rotation);
                }
            });
        }
    }
}
