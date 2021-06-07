using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_2 : MonoBehaviour
{
    [SerializeField] Transform playerPosition;
    [SerializeField] ParticleSystem exclamation;
    [SerializeField] Transform cameraPosition;
    [SerializeField] AudioClip bossBattleMusic;
    [SerializeField] float bossBattleMusicVolume;
    [SerializeField] float defaultDistance = 10;
    [SerializeField] float playerSpeed = 40;
    [SerializeField] AudioClip exclamationSound;
    [SerializeField] float exclamationSoundVolume;
    [SerializeField] float exclamationSoundPitch = 1;
    [SerializeField] GameObject princess;
    [SerializeField] GameObject[] battleColliders;


    private void Start()
    {
        princess.GetComponent<HealthController>().SetInvencible();
    }
    public void StartCutscene()
    {
        StartCoroutine(_StartCutscene());
    }
    IEnumerator _StartCutscene()
    {
       
        LevelManager.instance.camera.Follow = cameraPosition;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerUI.instance.HideUI();
        player.GetComponent<InputController>().enabled = false;
        player.GetComponent<GroundMovementController>().enabled = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        SoundManager.instance.FadeBGM(1);
        CutsceneManager.instance.StartCutscene();

        Animator playerAnimator = player.GetComponent<Animator>();

        playerAnimator.SetFloat("Speed", 31);
        while (!CutsceneManager.instance.MoveObjectToPosition(player.transform, playerPosition.transform.position, playerSpeed, defaultDistance))
        {
            yield return null;
        }
        playerAnimator.SetFloat("Speed", 0);

        exclamation.Play();
        exclamation.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 10, 0);
        SoundManager.instance.PlaySFX(exclamationSound, exclamationSoundVolume, exclamationSoundPitch);
        yield return new WaitForSeconds(.2f + exclamation.main.duration);
        princess.GetComponent<Animator>().SetTrigger("Angry");
        yield return new WaitForSeconds(.5f);
        SoundManager.instance.PlayBGM(bossBattleMusic, bossBattleMusicVolume);
        foreach (var item in battleColliders)
        {
            item.SetActive(true);
        }
        princess.GetComponent<BossAi>().TryStartBoss();
        player.GetComponent<InputController>().enabled = true;
        player.GetComponent<GroundMovementController>().enabled = true;
        PlayerUI.instance.ShowUI();
        CutsceneManager.instance.EndCutscene();
        princess.GetComponent<HealthController>().SetInvencible(false);

    }
}



