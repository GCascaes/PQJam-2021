using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_0 : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] ParticleSystem _exclamation;
    [SerializeField] ParticleSystem _music;
    [SerializeField] Transform _vulture;
    [SerializeField] Transform _vultureEndPosition;
    [SerializeField] Transform _vultureHalfEndPosition;
    [SerializeField] AudioClip vultureAudioClip;
    [SerializeField] float vultureAudioClipPitch;
    [SerializeField] float _vultureSpeed;
    [SerializeField] AudioClip levelMusic;
    [SerializeField] float _musicVolume;
    [SerializeField] AudioClip baloonClip;
    [SerializeField] float baloonClipClipPitch;




    IEnumerator Start()
    {
        WaitForSeconds halfSecond = new WaitForSeconds(.5f);
        _player.GetComponent<InputController>().enabled = false;
        yield return null;
        CutsceneManager.instance.StartCutscene();
        yield return ScreenFader.instance.FadeIn(.5f);
        yield return halfSecond;
        SoundManager.instance.PlaySFX(baloonClip, 1, baloonClipClipPitch);
        _music.Play();
        yield return new WaitForSeconds(_music.main.duration + .3f);
        SoundManager.instance.PlayBGM(vultureAudioClip, 1, vultureAudioClipPitch);
        while (!CutsceneManager.instance.MoveObjectToPosition(_vulture, _vultureHalfEndPosition.position, _vultureSpeed))
        {
            yield return null;
        }
        SoundManager.instance.PlaySFX(baloonClip, 1, baloonClipClipPitch);
        _exclamation.Play();

        while (!CutsceneManager.instance.MoveObjectToPosition(_vulture, _vultureEndPosition.position, _vultureSpeed))
        {
            yield return null;
        }
        SoundManager.instance.FadeBGM(.7f);
        
        Destroy(_vulture.gameObject);
        CutsceneManager.instance.EndCutscene();
        _player.GetComponent<InputController>().enabled = true;
        SoundManager.instance.PlayBGM(levelMusic,_musicVolume);



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
