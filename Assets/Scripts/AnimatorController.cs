using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorController : MonoBehaviour
{
    //[SerializeField] float flashPower = 3;
    [SerializeField] Material mat;

    Coroutine flashCoroutine;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if (mat == null)
            mat = GetComponent<SpriteRenderer>().material;
    }

    public void SetBool(string name, bool value)
    {
        anim.SetBool(name, value);
    }

    public void GetBool(string name)
    {
        anim.GetBool(name);
    }

    public void SetFloat(string name, float value)
    {
        anim.SetFloat(name, value);
    }

    public void GetFloat(string name)
    {
        anim.GetFloat(name);
    }

    public void SetTrigger(string name)
    {
        anim.SetTrigger(name);
    }

    public void ResetTrigger(string name)
    {
        anim.ResetTrigger(name);
    }

    public void Flash(float time = 1f, float flashPower = 3, float flashWaitTime = 0.05f)
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
