using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] Transform playerPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == GameObject.FindGameObjectWithTag("Player"))
        {
            GameManager.instance.hasCheckPoint = true;
            GameManager.instance.spawnPosition = playerPosition.position;
        }
    }
}
