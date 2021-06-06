using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cutscene_4 : MonoBehaviour
{
    [SerializeField] Renderer bg;
    [SerializeField] float bgSpeed;
    [SerializeField] float vultureSpeed;
    [SerializeField] AudioClip music;
    [SerializeField] float musicVolume;
    [SerializeField] Transform vulture;
    [SerializeField] Transform vultureEndPosition;
    [SerializeField] GameObject paper;
    [SerializeField] Transform paperEndPosition;
    [SerializeField] float paperSpeed;
    [SerializeField] Transform poster;
    [SerializeField] Transform posterEndPosition;
    [SerializeField] Image fadeImage;
    [SerializeField] float posterMoveTime;


    float time;
    float halfTime;
    Material bgmat;
    bool endGame;
    Vector2 offset;
    // Start is called before the first frame update
    void Start()
    {
        bgmat = bg.material;
        time = music.length;
        halfTime = time / 2;
        SoundManager.instance.PlayBGM(music, musicVolume,1,false);
        offset = new Vector2(bgSpeed, 0);
    }


    // Update is called once per frame
    void Update()
    {
        CutsceneManager.instance.MoveObjectToPosition(vulture, vultureEndPosition.position, vultureSpeed);
        bgmat.mainTextureOffset += offset * Time.deltaTime;
        time -= Time.deltaTime;
        if(time <= halfTime)
        {
            if (!paper.activeSelf)
                paper.SetActive(true);

            CutsceneManager.instance.MoveObjectToPosition(paper.transform, paperEndPosition.position, paperSpeed);
        }

        if (time <= 0)
            EndGame();
    }

    private void EndGame()
    {
        if (endGame)
            return;

        endGame = true;

        poster.DOLocalMove(posterEndPosition.localPosition, posterMoveTime).OnComplete(()=> { StartCoroutine(_FadeOut()); });
    }

    private IEnumerator _FadeOut(float timeToFadeOut = 1f)
    {
        fadeImage.gameObject.SetActive(true);
        float opacity = 0;
        float opacityToAdd = 1 / (timeToFadeOut * 100);

        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.01f);
        while (fadeImage.color.a < 1)
        {
            opacity += opacityToAdd;
            opacity = opacity > 1 ? 1 : opacity;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, opacity);
            yield return wait;
        }

        yield return new WaitForSeconds(2);
        

        yield return _FadeOut(2);
    }
}
