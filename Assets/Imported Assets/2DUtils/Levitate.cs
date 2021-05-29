using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitate : MonoBehaviour
{
    [SerializeField]float yVariation = 0.1f;
    Vector3 normalPosition;
    Vector3 floatPosition;
    Transform trans;
    bool stop;
    void Start()
    {
        trans = transform;
        normalPosition = trans.localPosition;
        floatPosition = new Vector3(normalPosition.x, normalPosition.y + yVariation, normalPosition.z);
        DoLevitate();
    }

    void DoLevitate()
    {
        Vector3 newPosition = (trans.localPosition == normalPosition)? floatPosition: normalPosition;
        if(!stop)
            trans.DOLocalMove(newPosition, 0.75f).OnComplete(DoLevitate);
    }

    internal void Stop()
    {
        stop = true;
    }
}
