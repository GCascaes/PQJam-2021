using System.Collections;
using UnityEngine;

public class FlashController : MonoBehaviour
{
    private Material material;
    private Coroutine flashCoroutine;

    private void Start()
    {
        if (TryGetComponent<SpriteRenderer>(out var renderer))
            material = renderer.material;
    }

    public void Flash(float time = .5f, float flashPower = 3, float flashWaitTime = 0.05f)
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(_Flash(time, flashPower, flashWaitTime));
    }

    private IEnumerator _Flash(float time, float flashPower, float flashWaitTime)
    {
        WaitForSeconds wait = new WaitForSeconds(flashWaitTime);
        float timeDecurred = 0;
        material.SetFloat("_FlashPower", 0);

        while (timeDecurred < time)
        {
            material.SetFloat("_FlashPower", flashPower);
            yield return wait;
            material.SetFloat("_FlashPower", 0);
            yield return wait;
            timeDecurred += 2 * flashWaitTime;
        }
    }
}
