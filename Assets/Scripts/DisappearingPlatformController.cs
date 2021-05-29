using System.Collections;
using UnityEngine;

public class DisappearingPlatformController : MonoBehaviour
{
    [SerializeField]
    private Collider2D colliderToDisable;
    [SerializeField]
    private SpriteRenderer spriteToDisable;
    [SerializeField]
    private float afterStepDuration;
    [SerializeField]
    private float reappearingTime;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopAllCoroutines();
        StartCoroutine(DisappearingRoutine());
    }

    private IEnumerator DisappearingRoutine()
    {
        yield return new WaitForSecondsRealtime(afterStepDuration);

        if (colliderToDisable != null)
            colliderToDisable.enabled = false;

        if (spriteToDisable != null)
            spriteToDisable.enabled = false;

        yield return new WaitForSecondsRealtime(reappearingTime);

        if (colliderToDisable != null)
            colliderToDisable.enabled = true;

        if (spriteToDisable != null)
            spriteToDisable.enabled = true;
    }
}
