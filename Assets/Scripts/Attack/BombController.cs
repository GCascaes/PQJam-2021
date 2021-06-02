using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private float minimumVelocityToExplode = 0;

    private Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LevelBounds")
            || collision.gameObject.CompareTag("Bullet")
            || (collision.collider.isTrigger && !collision.gameObject.CompareTag("DefenseBarrier"))
            || explosionPrefab is null
            || (body != null && body.velocity.magnitude < minimumVelocityToExplode))
            return;

        Instantiate(explosionPrefab, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
