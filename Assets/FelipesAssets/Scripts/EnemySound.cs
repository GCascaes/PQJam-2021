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

    private EnemyState currentState;

    private EnemyAi ai;

    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<EnemyAi>();
        currentState = ai.CurrentState;
    }

    // Update is called once per frame
    void Update()
    {
        currentState = ai.CurrentState;


        if (isAware)
        {
            isAware = false;

            SoundManager.instance.PlaySFX(awareClip, SoundManager.instance.sfxVolume, pitch + Random.Range(-0.07f, 0.07f));
        }



        if (isAttacking)
        {
            isAttacking = false;

            Invoke("EnemyGunSound", 0.5f);
        }


        if (currentState == EnemyState.SeenTarget)
        {
            isAware = true;
        }

        //if (currentState == EnemyState.AttackingTarget)
        //{
        //    isAttacking = true;

        //}


    }

    public void EnemyGunSound()
    {
        SoundManager.instance.PlaySFX(attackClip, SoundManager.instance.sfxVolume, pitch + Random.Range(-0.07f, 0.07f));
    }
}
