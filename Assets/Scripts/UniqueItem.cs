using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueItem : MonoBehaviour
{
    [SerializeField]int id;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in GameManager.instance.itemsId)
        {
            if (item == id)
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            GameManager.instance.itemsId.Add(id);
    }
}
