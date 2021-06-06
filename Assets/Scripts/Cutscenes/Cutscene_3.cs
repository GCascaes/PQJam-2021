using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_3 : MonoBehaviour
{
    [SerializeField] GameObject vulture;
    [SerializeField] Transform vultureFront;
    [SerializeField] Vector3 playerRotationToVulture;
    [SerializeField] Transform princess;
    [SerializeField] GameObject bagOfMoney;
    [SerializeField] float playerSpeed = 40;
    [SerializeField] AudioClip getSound;
    [SerializeField] float getSoundVolume;
    [SerializeField] float getSoundPitch = 1;
    [SerializeField] AudioClip endBossFightMusic;
    [SerializeField] float endBossFightMusicVolume;


    Animator vultureAnimator;

    void Start()
    {
        vultureAnimator = vulture.GetComponent<Animator>();
        vultureAnimator.SetTrigger("Down");
    }

    IEnumerator _StartCutscene()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerUI.instance.HideUI();
        player.GetComponent<InputController>().enabled = false;
        player.GetComponent<HealthController>().EndLevel();
        SoundManager.instance.FadeBGM(1);
        yield return new WaitForSeconds(0.7f);
        SoundManager.instance.PlayBGM(endBossFightMusic, endBossFightMusicVolume, 1, false);
        yield return new WaitForSeconds(endBossFightMusic.length);

        CutsceneManager.instance.StartCutscene();
        bagOfMoney.transform.position = princess.transform.position;
        bagOfMoney.SetActive(true);
        while (!CutsceneManager.instance.MoveObjectToPosition(player.transform, bagOfMoney.transform.position, playerSpeed, 1))
        {
            yield return null;
        }
        SoundManager.instance.PlaySFX(getSound, getSoundVolume, getSoundPitch);
        yield return new WaitForSeconds(getSound.length+0.2f);
        while (!CutsceneManager.instance.MoveObjectToPosition(player.transform, vultureFront.position, playerSpeed, 1))
        {
            yield return null;
        }
        player.transform.rotation = Quaternion.Euler(playerRotationToVulture);
        yield return new WaitForSeconds(0.35f);
    }
}
