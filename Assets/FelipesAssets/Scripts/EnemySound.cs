using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{

    [SerializeField]
    private bool isAttacking;

    [SerializeField]
    private bool isAware;

    [SerializeField]
    private AudioClip attackClip;

    [SerializeField]
    private AudioClip awareClip;

    [SerializeField]
    private float volume = 0.7f;

    [SerializeField]
    private float pitch = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isAware)
        {
            isAware = false;

            SoundManager.instance.PlaySFX(awareClip, volume, pitch + Random.Range(-0.07f, 0.07f));
        }


        if (isAttacking)
        {
            isAttacking = false;

            SoundManager.instance.PlaySFX(attackClip, volume, pitch + Random.Range(-0.07f, 0.07f));
        }
    }
}
