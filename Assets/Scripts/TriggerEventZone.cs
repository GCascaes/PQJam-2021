using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventZone : MonoBehaviour
{
    [SerializeField]UnityEvent onEnter;

    GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player && onEnter != null)
            onEnter.Invoke();


    }
}
