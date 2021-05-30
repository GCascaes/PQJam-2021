using UnityEngine;

public class DeathHole : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<HealthController>(out var healthController))
            healthController.TakeDamage(float.MaxValue);
        else
            Destroy(collision.gameObject);
    }
}
