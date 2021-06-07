using System.Collections;
using System.Linq;
using UnityEngine;

public class ContactDamageController : MonoBehaviour
{
    [SerializeField]
    private string targetTag;
    [SerializeField]
    private float contactDamage;
    [SerializeField]
    private bool percentualDamage;
    [SerializeField]
    private Collider2D contactDamageCollider;
    [SerializeField]
    private bool disableCollisionsWithTarget = true;

    private float currentContactDamage;

    public Collider2D ContactDamageCollider => contactDamageCollider;

    private void Awake()
    {
        currentContactDamage = contactDamage;
    }

    private void OnEnable()
    {
        if (!disableCollisionsWithTarget)
            return;

        var myColliders = GetComponents<Collider2D>().Where(x => !x.isTrigger);
        
        var colliders = GameObject
            .FindGameObjectsWithTag(targetTag)
            .SelectMany(x => x.GetComponents<Collider2D>().Where(x => !x.isTrigger))
            .ToList();

        foreach (var collider in colliders)
            foreach (var myCollider in myColliders)
                Physics2D.IgnoreCollision(collider, myCollider);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!disableCollisionsWithTarget)
            return;

        if (collision.gameObject.CompareTag(targetTag))
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
    }

    private void OnTriggerEnter2D(Collider2D collision) => TryDamage(collision);

    private void OnTriggerStay2D(Collider2D collision) => TryDamage(collision);

    private void TryDamage(Collider2D collider)
    {
        if (collider == null || contactDamage <= 0 || !enabled)
            return;

        if (!collider.gameObject.CompareTag(targetTag)
            || contactDamageCollider == null
            || !collider.IsTouching(contactDamageCollider))
            return;

        if (collider.gameObject.TryGetComponent<HealthController>(out var healthController))
        {
            if (percentualDamage)
                healthController.TakeDamagePercent(currentContactDamage);
            else
                healthController.TakeDamage(currentContactDamage);
        }
    }

    public void AlterDamage(float newDamage, float duration, bool isPercentual = false)
    {
        StartCoroutine(AlterDamageTemporarily(newDamage, duration, isPercentual));
    }

    private IEnumerator AlterDamageTemporarily(float newDamage, float duration, bool isPercentual)
    {
        var oldPercentual = percentualDamage;
        percentualDamage = isPercentual;
        currentContactDamage = newDamage;
        yield return new WaitForSecondsRealtime(duration);
        currentContactDamage = contactDamage;
        percentualDamage = oldPercentual;
    }
}
