using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    private float projectileDamage;
    [SerializeField]
    private GameObject shotParticlePrefab;
    [SerializeField]
    private GameObject hitParticlePrefab;

    [SerializeField]
    private AudioClip spawnAudio;

    [SerializeField]
    [Range(0, 1f)]
    private float volume;

    [SerializeField]
    [Range(-1, 1)]
    private float pitch;

    private float bulletVelocity;
    
    protected string ShootingEntityTag { get; private set; }

    public static ProjectileController Instantiate(
        GameObject projectilePrefab,
        Vector2 position,
        Quaternion rotation,
        float bulletVelocity,
        string shootingEntityTag)
    {
        var projectile = Instantiate(projectilePrefab, position, rotation);
        var controller = projectile.GetComponent<ProjectileController>();
        controller.bulletVelocity = bulletVelocity;
        controller.ShootingEntityTag = shootingEntityTag;
        return controller;
    }

    private void Start()
    {
        var body = GetComponent<Rigidbody2D>();
        body.velocity = body.transform.right * bulletVelocity;

        if (shotParticlePrefab != null)
            Instantiate(shotParticlePrefab, transform.position, transform.rotation);

        if (spawnAudio != null)
            SoundManager.instance.PlaySFX(spawnAudio, volume, pitch);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LevelBounds")
            || collision.CompareTag("Bullet")
            || collision.CompareTag(ShootingEntityTag)
            || (collision.isTrigger && !collision.CompareTag("DefenseBarrier")))
            return;

        if (collision.gameObject.TryGetComponent<HealthController>(out var healthController))
            healthController.TakeDamage(projectileDamage);

        // TODO Add camera shake

        if (hitParticlePrefab != null)
            Instantiate(hitParticlePrefab, transform.position, transform.rotation);

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
