using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float playerHealth;
    public int numOfHearts = 3;
    public int numOfShields = 3;
    public int numOfDeaths { get; private set; }

    bool deathStarted;
    public static GameManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Death()
    {
        if (deathStarted)
            return;
        deathStarted = true;
        numOfDeaths++;
        StartCoroutine(_Death());
    }

    private IEnumerator _Death()
    {
        
        WaitForSeconds wait = new WaitForSeconds(1f);
        yield return wait;
        SoundManager.instance.FadeBGM(1);
        yield return ScreenFader.instance.FadeOut(1f);
        yield return wait;
        string sceneToLoad = SceneManager.GetActiveScene().name;
        Loading.LoadScene(sceneToLoad);
        deathStarted = false;

    }
}
