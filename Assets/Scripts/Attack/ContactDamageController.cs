using UnityEngine;

public class ContactDamageController : MonoBehaviour
{
    [SerializeField]
    private string targetTag;
    [SerializeField]
    private float contactDamage;
    [SerializeField]
    private float collisionStayDamageAgainTime;
    [SerializeField]
    private Collider2D contactDamageCollider;

    private float lastDamageTime = 0;

    private void OnCollisionEnter2D(Collision2D collision) => TryDamage(collision);

    private void OnCollisionStay2D(Collision2D collision) => TryDamage(collision);

    private void TryDamage(Collision2D collision)
    {
        if (Time.realtimeSinceStartup - lastDamageTime < collisionStayDamageAgainTime)
            return;

        if (!collision.gameObject.CompareTag(targetTag)
            || contactDamageCollider is null
            || collision.otherCollider != contactDamageCollider)
            return;

        if (collision.gameObject.TryGetComponent<HealthController>(out var healthController))
            healthController.TakeDamage(contactDamage);

        lastDamageTime = Time.realtimeSinceStartup;
    }
}
