using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_1 : MonoBehaviour
{
    GameObject _player;
    [SerializeField] Transform _playerEndPosition;
    [SerializeField] float _playerSpeed;
    [SerializeField] Transform tutorialBear;
    [SerializeField] GameObject bazookaShotPrefab;


    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == _player)
            StartCoroutine(_StartCutscene());
    }
    IEnumerator _StartCutscene()
    {
        PlayerUI.instance.HideUI();
        _player.GetComponent<InputController>().enabled = false;
        yield return null;
        CutsceneManager.instance.StartCutscene();
        while (!CutsceneManager.instance.MoveObjectToPosition(_player.transform, _playerEndPosition.position, _playerSpeed))
        {
            yield return null;
        }
        Animator tutorialBearAnim = tutorialBear.GetComponent<Animator>();
        tutorialBearAnim.SetBool("isAngry", true);
        yield return new WaitForSeconds(1.75f);
        tutorialBearAnim.SetBool("Shot", true);
        Instantiate(bazookaShotPrefab, tutorialBear.GetChild(0));

        yield return new WaitForSeconds(1f);
        tutorialBearAnim.SetBool("Shot", false);


    }
}
