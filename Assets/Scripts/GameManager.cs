using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
    }
}
