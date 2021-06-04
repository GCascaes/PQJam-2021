using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject uiParent;
    [SerializeField] GameObject heartPrefab;
    [SerializeField] GameObject shieldPrefab;
    [SerializeField] Transform heartParent;
    [SerializeField] Transform shieldParent;

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
        for (int i = 0; i < GameManager.instance.numOfHearts; i++)
        {
            AddHeart();
        }
        for (int i = 0; i < GameManager.instance.numOfShields; i++)
        {
            AddShield();
        }
    }

    public void UpdateHeartBar(float maxHp, float currentHP)
    {
        Debug.Log(currentHP);
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
}
