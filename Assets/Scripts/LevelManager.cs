using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] protected AudioClip _levelMusic;
    [SerializeField] protected float _musicVolume;
    [SerializeField] CinemachineVirtualCamera _camera;
    [SerializeField] Transform playerEndLevelStartPosition;
    [SerializeField] Transform playerEndLevelEndPosition;
    [SerializeField] float playerEndLevelSpeed = 30;

    public AudioClip levelMusic { get { return _levelMusic; } }
    public float musicVolume { get { return _musicVolume; } }
    public CinemachineVirtualCamera camera { get { return _camera; } }
    public static LevelManager instance { get; protected set; }
    protected void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        LevelStart();    
    }

    protected virtual void LevelStart()
    {
        SoundManager.instance.PlayBGM(_levelMusic, _musicVolume);
    }

    public virtual void LevelEnd()
    {
        StartCoroutine(_LevelEnd());
    }

    private IEnumerator _LevelEnd()
    {
        SoundManager.instance.FadeBGM(.5f);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<InputController>().enabled = false;
        while (!CutsceneManager.instance.MoveObjectToPosition(player.transform, playerEndLevelStartPosition.position, playerEndLevelSpeed))
        {
            yield return null;
        }
        yield return null;
        player.GetComponent<Animator>().SetBool("Win", true);
        SoundManager.instance.PlaySFX(GameManager.instance.endLevelMusic, GameManager.instance.endLevelMusicVolume);
        yield return new WaitForSeconds(GameManager.instance.endLevelMusic.length);
        player.GetComponent<Animator>().SetBool("Win", false);

        StartCoroutine(ScreenFader.instance.FadeOut(1));
        while (!CutsceneManager.instance.MoveObjectToPosition(player.transform, playerEndLevelEndPosition.position, playerEndLevelSpeed))
        {
            yield return null;
        }
    }
}
