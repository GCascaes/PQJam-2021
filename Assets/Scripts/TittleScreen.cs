using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TittleScreen : MonoBehaviour
{
    [SerializeField] Image button;
    [SerializeField] Sprite[] sprites;
    [SerializeField] float animationTime = .3f;

    int index;
    float currentTime;
    bool pressedButton;


    // Update is called once per frame
    void Update()
    {
        Animate();
        if (Input.GetButton("Jump"))
            StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        if (pressedButton)
            yield break;
        pressedButton = true;
        yield return ScreenFader.instance.FadeOut();
        Loading.LoadScene("Level_0");
    }

    private void Animate()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= animationTime)
        {
            index++;

            if (index >= sprites.Length)
                index = 0;

            button.sprite = sprites[index];

            currentTime = 0;
        }
    }
}
