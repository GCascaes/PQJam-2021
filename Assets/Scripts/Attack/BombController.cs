using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private bool isArmed = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LevelBounds")
            || collision.gameObject.CompareTag("Bullet")
            || (collision.collider.isTrigger && !collision.gameObject.CompareTag("DefenseBarrier"))
            || explosionPrefab is null
            || !isArmed)
            return;

        Instantiate(explosionPrefab, transform.position, transform.rotation);

        Destroy(gameObject);
    }

    public void Arm() => isArmed = true;
}
