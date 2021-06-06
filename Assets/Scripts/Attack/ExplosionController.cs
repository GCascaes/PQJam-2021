using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField]
    private float damage;

    [SerializeField]
    private AudioClip spawnAudio;

    [SerializeField]
    [Range(0, 1f)]
    private float volume;

    [SerializeField]
    [Range(-1, 1)]
    private float pitch;

    private void Start()
    {
        SoundManager.instance.PlaySFX(spawnAudio, volume, pitch);

    }

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
