using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevelManager : LevelManager
{
    [SerializeField] int numOfDeathsToSpawnGun = 2;
    [SerializeField] GameObject gunSign;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject bazookaTrueEnemy;
    //[SerializeField] GameObject bazookaTutorialEnemy;
    [SerializeField] GameObject cutscene;

    protected override void LevelStart()
    {
        if(GameManager.instance.numOfDeaths >0)
            SoundManager.instance.PlayBGM(_levelMusic,_musicVolume);
        
        if(GameManager.instance.numOfDeaths >= numOfDeathsToSpawnGun)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetBool("Angry", true);
            gunSign.SetActive(true);
            gun.SetActive(true);
            //bazookaTutorialEnemy.SetActive(false);
            bazookaTrueEnemy.SetActive(true);
            cutscene.SetActive(false);
        }
    }


}
