using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] Transform _playerEndLevelStartPosition;
    [SerializeField] Transform _playerEndLevelEndPosition;

    public Vector3 playerEndLevelStartPosition { get { return _playerEndLevelStartPosition.position; } }
    public Vector3 playerEndLevelEndPosition { get { return _playerEndLevelEndPosition.position; } }
    public void LevelEndExec()
    {
        LevelManager.instance.LevelEnd(this);
    }
}
