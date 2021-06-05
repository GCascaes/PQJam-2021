using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_0 : MonoBehaviour
{
    GameObject _player;
    [SerializeField] float _playerSpeed;
    
    [SerializeField] Transform _playerEndPosition;
    [SerializeField] ParticleSystem _exclamation;
    [SerializeField] ParticleSystem _music;
    [SerializeField] Transform _vulture;
    [SerializeField] Transform _vultureEndPosition;
    [SerializeField] Transform _vultureHalfEndPosition;
    [SerializeField] AudioClip vultureAudioClip;
    [SerializeField] float vultureAudioClipPitch;
    [SerializeField] float _vultureSpeed;
    //[SerializeField] AudioClip levelMusic;
    //[SerializeField] float _musicVolume;
    [SerializeField] AudioClip baloonClip;
    [SerializeField] float baloonClipClipPitch;




    IEnumerator Start()
    {
        if (GameManager.instance.numOfDeaths > 0)
            yield break;

        _player = GameObject.FindGameObjectWithTag("Player");
        PlayerUI.instance.HideUI();
        _player.GetComponent<InputController>().enabled = false;
        yield return null;
        CutsceneManager.instance.StartCutscene();
        //yield return ScreenFader.instance.FadeIn(.5f);
        yield return new WaitForSeconds(1.4f);
        SoundManager.instance.PlaySFX(baloonClip, 1, baloonClipClipPitch);
        _music.Play();
        yield return new WaitForSeconds(_music.main.duration + .3f);
        SoundManager.instance.PlayBGM(vultureAudioClip, 1, vultureAudioClipPitch);
        while (!CutsceneManager.instance.MoveObjectToPosition(_vulture, _vultureHalfEndPosition.position, _vultureSpeed))
        {
            yield return null;
        }
        LevelManager.instance.camera.Follow = null;
        StartCoroutine(MovePlayer());
        

        while (!CutsceneManager.instance.MoveObjectToPosition(_vulture, _vultureEndPosition.position, _vultureSpeed))
        {
            yield return null;
        }
        SoundManager.instance.FadeBGM(.7f);
        
        Destroy(_vulture.gameObject);
        CutsceneManager.instance.EndCutscene();
        _player.GetComponent<InputController>().enabled = true;
        PlayerUI.instance.ShowUI();
        SoundManager.instance.PlayBGM(LevelManager.instance.levelMusic , LevelManager.instance.musicVolume);
        LevelManager.instance.camera.Follow = _player.transform;
    }

    private IEnumerator MovePlayer()
    {
        while (!CutsceneManager.instance.MoveObjectToPosition(_player.transform, _playerEndPosition.position, _playerSpeed))
        {
            yield return null;
        }
        SoundManager.instance.PlaySFX(baloonClip, 1, baloonClipClipPitch);
        _exclamation.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
