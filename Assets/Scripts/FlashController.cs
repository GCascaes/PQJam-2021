using System.Collections;
using UnityEngine;

public class FlashController : MonoBehaviour
{
    //[SerializeField] float flashPower = 3;
    [SerializeField] SpriteRenderer renderer;

    Material mat;
    Coroutine flashCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        if (renderer == null)
            renderer = GetComponent<SpriteRenderer>();

        mat = renderer.material;
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
        mat.SetFloat("_FlashPower", 0);

        while (timeDecurred < time)
        {
            mat.SetFloat("_FlashPower", flashPower);
            yield return wait;
            mat.SetFloat("_FlashPower", 0);
            yield return wait;
            timeDecurred+= (flashWaitTime*2);
        }

    }
}
