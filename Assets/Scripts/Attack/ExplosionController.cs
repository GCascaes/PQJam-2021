using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField]
    private float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LevelBounds")
            || collision.CompareTag("Bullet")
            || collision.isTrigger)
            return;

        if (collision.gameObject.TryGetComponent<HealthController>(out var healthController))
            healthController.TakeDamage(damage);
    }
}
