using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnPrefab;
    [SerializeField]
    private float spawnPeriod;

    private void Awake()
    {
        StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Spawn()
    {
        while (isActiveAndEnabled)
        {
            Instantiate(spawnPrefab, transform.position, transform.rotation);
            yield return new WaitForSecondsRealtime(spawnPeriod);
        }
    }
}
