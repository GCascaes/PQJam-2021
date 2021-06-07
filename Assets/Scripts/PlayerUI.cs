using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject uiParent;
    [SerializeField] GameObject heartPrefab;
    [SerializeField] GameObject shieldPrefab;
    [SerializeField] Transform heartParent;
    [SerializeField] Transform shieldParent;
    [SerializeField] Image powerUpSprite;
    [SerializeField] PowerUpSprite[] powerUps;
    List<GameObject> hearts = new List<GameObject>();
    List<GameObject> shields = new List<GameObject>();

    static PlayerUI _instance;
    public static PlayerUI instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        powerUpSprite.gameObject.SetActive(false);

        for (int i = 0; i < GameManager.instance.numOfHearts; i++)
        {
            AddHeart();
        }
        //for (int i = 0; i < GameManager.instance.numOfShields; i++)
        //{
        //    AddShield();
        //}
    }

    public void UpdateHeartBar(float maxHp, float currentHP)
    {
        float hpPercentage = currentHP / maxHp;

        float percentagePerHeart = 1f/(float)hearts.Count;

        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].transform.GetChild(0).GetComponent<Image>().fillAmount = hpPercentage / percentagePerHeart;
            hpPercentage -= percentagePerHeart;
        }
    }
    

    public void AddHeart()
    {
        GameObject go = Instantiate(heartPrefab, heartParent);
        hearts.Add(go);
        go.SetActive(true);
    }
    private void AddShield()
    {
        GameObject go = Instantiate(shieldPrefab, shieldParent);
        go.SetActive(true);
        shields.Add(go);
    }
    public void HideUI()
    {
        uiParent.SetActive(false);
    }
    public void ShowUI()
    {
        uiParent.SetActive(true);
    }

    public void SetPowerUpUI (PowerUpType powerUp)
    {
        if (powerUp == PowerUpType.None)
            powerUpSprite.gameObject.SetActive(false);
        else
            powerUpSprite.gameObject.SetActive(true);

        powerUpSprite.sprite = powerUps
            .Where(x => x.powerUp == powerUp)
            .SingleOrDefault()
            .sprite;
    }

    [Serializable]
    public class PowerUpSprite
    {
        public PowerUpType powerUp;
        public Sprite sprite;
    }
}
