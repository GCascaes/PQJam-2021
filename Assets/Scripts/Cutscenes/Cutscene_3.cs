using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_3 : MonoBehaviour
{
    [SerializeField] Transform cameraPosition;
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
    [SerializeField] AudioClip untieSound;
    [SerializeField] float untieSoundVolume;
    [SerializeField] float untieSoundPitch = 1;
    [SerializeField] AudioClip thumbsSound;
    [SerializeField] float thumbsSoundVolume;
    [SerializeField] float thumbsSoundPitch = 1;
    [SerializeField] GameObject guards;
    [SerializeField] Transform guardsFinalPosition;
    [SerializeField] ParticleSystem[] exclamation;
    [SerializeField] Vector3 rotationToEntrance;
    [SerializeField] Vector3 rotationToExit;
    [SerializeField] AudioClip exclamationSound;
    [SerializeField] float exclamationSoundVolume;
    [SerializeField] float exclamationSoundPitch = 1;
    [SerializeField] AudioClip flySound;
    [SerializeField] float flySoundVolume;
    [SerializeField] float flySoundPitch = .7f;
    [SerializeField] Transform vultureExitLevelPosition;
    [SerializeField] float vultureSpeed = 55;
    [SerializeField] float defaultDistance = 10;
    [SerializeField] AudioClip moneySound;
    [SerializeField] float moneySoundVolume;
    [SerializeField] float moneySoundPitch = 1;

    Animator vultureAnimator;
    Animator[] guardsAnimator;

    void Start()
    {
        vultureAnimator = vulture.GetComponent<Animator>();
        vultureAnimator.SetTrigger("Down");
        guardsAnimator = new Animator[guards.transform.childCount];
        for (int i = 0; i < guardsAnimator.Length; i++)
        {
            guardsAnimator[i] = guards.transform.GetChild(i).GetComponent<Animator>();
        }
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
        player.GetComponent<HealthController>().EndLevel();
        SoundManager.instance.FadeBGM(1);
        yield return new WaitForSeconds(0.7f);

        Animator playerAnimator = player.GetComponent<Animator>();
        playerAnimator.SetBool("Win", true);
        SoundManager.instance.PlayBGM(endBossFightMusic, endBossFightMusicVolume, 1, false);
        yield return new WaitForSeconds(endBossFightMusic.length);
        playerAnimator.SetBool("Angry", false);
        playerAnimator.SetBool("HasGun", false);
        playerAnimator.SetBool("Win", false);
        

        CutsceneManager.instance.StartCutscene();
        SoundManager.instance.PlaySFX(moneySound, moneySoundVolume, moneySoundPitch);

        bagOfMoney.transform.position = new Vector3(princess.transform.position.x, princess.transform.position.y+10, princess.transform.position.z);
        bagOfMoney.SetActive(true);

        while (!CutsceneManager.instance.MoveObjectToPosition(bagOfMoney.transform, princess.transform.position, playerSpeed, defaultDistance))
        {
            yield return null;
        }
        //yield return new WaitForSeconds(.5f);

        if (player.transform.position.x > bagOfMoney.transform.position.x)
            player.transform.rotation = Quaternion.Euler(rotationToEntrance);
        else
            player.transform.rotation = Quaternion.Euler(rotationToExit);

        playerAnimator.SetFloat("Speed", 31);
        while (!CutsceneManager.instance.MoveObjectToPosition(player.transform, bagOfMoney.transform.position, playerSpeed, defaultDistance))
        {
            yield return null;
        }
        playerAnimator.SetFloat("Speed", 0);
        SoundManager.instance.PlaySFX(getSound, getSoundVolume, getSoundPitch);
        Destroy(bagOfMoney);
        yield return new WaitForSeconds(getSound.length+0.2f);

        if (player.transform.position.x > vultureFront.position.x)
            player.transform.rotation = Quaternion.Euler(rotationToEntrance);
        else
            player.transform.rotation = Quaternion.Euler(rotationToExit);
        playerAnimator.SetFloat("Speed", 31);
        while (!CutsceneManager.instance.MoveObjectToPosition(player.transform, vultureFront.position, playerSpeed, defaultDistance))
        {
            yield return null;
        }
        playerAnimator.SetFloat("Speed", 0);
        player.transform.rotation = Quaternion.Euler(playerRotationToVulture);
        yield return new WaitForSeconds(0.35f);
        SoundManager.instance.PlaySFX(untieSound, untieSoundVolume, untieSoundPitch);
        yield return new WaitForSeconds(untieSound.length);
        vultureAnimator.SetTrigger("Idle");
        yield return new WaitForSeconds(0.5f);
        vultureAnimator.SetTrigger("Thanks");
        playerAnimator.SetBool("Win", true);
        vulture.GetComponent<SpriteRenderer>().sortingOrder = 7;

        SoundManager.instance.PlaySFX(thumbsSound, thumbsSoundVolume, thumbsSoundPitch);
        yield return new WaitForSeconds(thumbsSound.length);

        guards.SetActive(true);
        foreach (var item in guardsAnimator)
        {
            item.SetFloat("Speed", 0.2f);
        }

        while (!CutsceneManager.instance.MoveObjectToPosition(guards.transform, guardsFinalPosition.position, playerSpeed, defaultDistance))
        {
            yield return null;
        }
        foreach (var item in guardsAnimator)
        {
            item.SetFloat("Speed", 0);
        }

        playerAnimator.SetBool("Win", false);
        vultureAnimator.SetTrigger("Idle");

        player.transform.rotation = Quaternion.Euler(rotationToEntrance);
        foreach (var item in exclamation)
            item.Play();
        SoundManager.instance.PlaySFX(exclamationSound, exclamationSoundVolume, exclamationSoundPitch);
        foreach (var item in guardsAnimator)
        {
            item.SetBool("Shot", true);
        }
        yield return new WaitForSeconds(.2f + exclamation[0].main.duration);
        player.transform.rotation = Quaternion.Euler(rotationToExit);
        vulture.transform.rotation = Quaternion.Euler(rotationToExit);

        playerAnimator.SetFloat("Speed", 31);
        while (!CutsceneManager.instance.MoveObjectToPosition(player.transform, vulture.transform.position, playerSpeed, defaultDistance))
        {
            yield return null;
        }
        playerAnimator.SetFloat("Speed", 0);
        //player.GetComponent<Rigidbody2D>().isKinematic = true;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        //player.GetComponent<Rigidbody2D>().gravityScale = 0;
        while (!CutsceneManager.instance.MoveObjectToPosition(player.transform, new Vector3(vulture.transform.position.x, vulture.transform.position.y+2, 0), playerSpeed))
        {
            yield return null;
        }
        player.transform.parent = vulture.transform;
        vultureAnimator.SetTrigger("Fly");
        SoundManager.instance.PlayBGM(flySound, flySoundVolume, flySoundPitch);
        while (!CutsceneManager.instance.MoveObjectToPosition(vulture.transform, vultureExitLevelPosition.position, vultureSpeed, defaultDistance))
        {
            yield return null;
        }
        SoundManager.instance.FadeBGM(1);
        yield return ScreenFader.instance.FadeOut(1);
        Loading.LoadScene("EndGame");
    }
}
