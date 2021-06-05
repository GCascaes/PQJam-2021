using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_1 : MonoBehaviour
{
    GameObject player;
    [SerializeField] Transform playerEndPosition;
    [SerializeField] float playerSpeed;
    [SerializeField] Transform tutorialBear;
    [SerializeField] GameObject bazookaShotPrefab;
    [SerializeField] ParticleSystem explosionParticle;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] float explosionPitch;
    [SerializeField] float bazookaSpeed;
    [SerializeField] float bazookaSpeedMultiplier =1.1f;



    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == player)
            StartCoroutine(_StartCutscene());
    }
    IEnumerator _StartCutscene()
    {
        PlayerUI.instance.HideUI();
        player.GetComponent<InputController>().enabled = false;
        yield return null;
        CutsceneManager.instance.StartCutscene();
        while (!CutsceneManager.instance.MoveObjectToPosition(player.transform, playerEndPosition.position, playerSpeed))
        {
            yield return null;
        }
        Animator tutorialBearAnim = tutorialBear.GetComponent<Animator>();
        tutorialBearAnim.SetBool("isAngry", true);
        yield return new WaitForSeconds(1.75f);
        tutorialBearAnim.SetBool("Shot", true);
        Transform bazooca = Instantiate(bazookaShotPrefab, tutorialBear.GetChild(0)).transform;
        while (!CutsceneManager.instance.MoveObjectToPosition(bazooca, playerEndPosition.position, bazookaSpeed))
        {
            bazookaSpeed *= bazookaSpeedMultiplier;
            yield return null;
        }
        Destroy(bazooca.gameObject);
        SoundManager.instance.PlaySFX(explosionSound,1, explosionPitch);
        explosionParticle.Play();
        yield return new WaitForSeconds(.2f);
        player.GetComponent<HealthController>().TakeDamage(9999999);
        tutorialBearAnim.SetBool("Shot", false);
    }
}
