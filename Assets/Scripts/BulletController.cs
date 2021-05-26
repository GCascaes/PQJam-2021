using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private float bulletDamage;
    [SerializeField]
    private GameObject shotParticlePrefab;
    [SerializeField]
    private GameObject hitParticlePrefab;

    private float bulletVelocity;

    public static BulletController Instantiate(
        GameObject projectilePrefab,
        Vector2 position,
        Quaternion rotation,
        float bulletVelocity)
    {
        var projectile = Instantiate(projectilePrefab, position, rotation);
        var controller = projectile.GetComponent<BulletController>();
        controller.bulletVelocity = bulletVelocity;
        return controller;
    }

    private void Start()
    {
        var body = GetComponent<Rigidbody2D>();
        body.velocity = body.transform.right * bulletVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("LevelBounds"))
            return;

        if (collision.gameObject.TryGetComponent<HealthController>(out var healthController))
        {
            healthController.TakeDamage(bulletDamage);
        }

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
