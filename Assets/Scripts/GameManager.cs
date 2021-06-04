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


    static GameManager _instance;

    public static GameManager instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Death()
    {
        numOfDeaths++;
        StartCoroutine(_Death());
    }

    private IEnumerator _Death()
    {
        yield return new WaitForSeconds(1f);
        yield return ScreenFader.instance.FadeOut(1f);
        Loading.LoadScene(SceneManager.GetActiveScene().name);
    }
}
