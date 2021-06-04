using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    static CutsceneManager _instance;
    public static CutsceneManager instance { get { return _instance; } }
    [SerializeField] Transform topCut;
    [SerializeField] Transform bottomCut;

    Vector3 topCutOnPosition;
    Vector3 topCutOffPosition;
    Vector3 bottomCutOnPosition;
    Vector3 bottomCutOffPosition;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        //topCutOnPosition = topCut.localPosition;
        //bottomCutOnPosition = bottomCut.localPosition;
        //topCutOffPosition = new Vector3(topCutOnPosition.x, topCutOnPosition.y + 100, topCutOnPosition.z);
        //bottomCutOffPosition = new Vector3(bottomCutOnPosition.x, bottomCutOnPosition.y - 100, bottomCutOnPosition.z);
        //topCut.localPosition = topCutOffPosition;
        //bottomCut.localPosition = bottomCutOffPosition;
    }

    private void Start()
    {
        topCutOnPosition = topCut.localPosition;
        bottomCutOnPosition = bottomCut.localPosition;
        topCutOffPosition = new Vector3(topCutOnPosition.x, topCutOnPosition.y + 100, topCutOnPosition.z);
        bottomCutOffPosition = new Vector3(bottomCutOnPosition.x, bottomCutOnPosition.y - 100, bottomCutOnPosition.z);
        topCut.localPosition = topCutOffPosition;
        bottomCut.localPosition = bottomCutOffPosition;
    }

    public void StartCutscene()
    {
        topCut.DOKill();
        topCut.DOLocalMove(topCutOnPosition, 0.2f);
        bottomCut.DOKill();
        bottomCut.DOLocalMove(bottomCutOnPosition, 0.2f);
    }

    public void EndCutscene()
    {
        topCut.DOKill();
        topCut.DOLocalMove(topCutOffPosition, 0.2f);
        bottomCut.DOKill();
        bottomCut.DOLocalMove(bottomCutOffPosition, 0.2f);
    }

    public bool MoveObjectToPosition(Transform obj, Vector3 position, float moveSpeed)
    {
        var movementThisFrame = moveSpeed * Time.deltaTime;
        obj.position = Vector2.MoveTowards(obj.position, position, movementThisFrame);
        return (Vector3.Distance(obj.position, position) <= 0.2f);
    }
}
