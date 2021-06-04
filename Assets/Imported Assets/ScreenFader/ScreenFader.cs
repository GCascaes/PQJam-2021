using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    [SerializeField] Image flashImage;
    [SerializeField] float startFadeInTime = 0.5f;
    [SerializeField] bool fadeInOnStart = true;
    bool executeFadeIn;
    bool flashing;

    static ScreenFader _instance;
    public static ScreenFader instance { get { return _instance; } }

    public delegate void OnFadeEnd();
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }
    void Start()
    {
        if(fadeInOnStart)
            StartCoroutine(FadeIn(startFadeInTime));
    }

    public IEnumerator FadeIn(float timeToFadeIn, OnFadeEnd callback = null)
    {
        executeFadeIn = true;

        float opacity = 1;
        float opacityToRemove = 1 / (timeToFadeIn * 100);

        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.01f);
        while(fadeImage.color.a > 0)
        {
            if (!executeFadeIn)
                yield break;

            opacity -= opacityToRemove;
            opacity = opacity < 0 ? 0 : opacity;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, opacity);
            yield return wait;
        }
        fadeImage.gameObject.SetActive(false);
        if (callback != null)
            callback.Invoke();
    }

    public IEnumerator FadeOut(float timeToFadeOut, OnFadeEnd callback = null)
    {
        executeFadeIn = false;

        fadeImage.gameObject.SetActive(true);
        float opacity = 0;
        float opacityToAdd = 1 / (timeToFadeOut * 100);

        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.01f);
        while (fadeImage.color.a < 1)
        {
            if (executeFadeIn)
                yield break;

            opacity += opacityToAdd;
            opacity = opacity > 1 ? 1 : opacity;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, opacity);
            yield return wait;
        }
        if (callback != null)
            callback.Invoke();
    }
    public IEnumerator Flash(Color color, float time)
    {
        if (flashing)
            yield break;

        flashing = true;
        Color defaultColor = flashImage.color;

        flashImage.color = color;

        flashImage.gameObject.SetActive(true);
        float opacity = 1;
        float opacityToRemove = 1 / (time * 100);

        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.01f);
        while (flashImage.color.a > 0)
        {
            if (!executeFadeIn)
                yield break;

            opacity -= opacityToRemove;
            opacity = opacity < 0 ? 0 : opacity;
            flashImage.color = new Color(flashImage.color.r, flashImage.color.g, flashImage.color.b, opacity);
            yield return wait;
        }
        flashImage.gameObject.SetActive(false);

        flashImage.color = defaultColor;
        flashing = false;
    }
}
