using System.Collections;
using System.Linq;
using UnityEngine;

public class SimpleShotAttackAi : ShotAttackBaseAi, IAttackAi
{
    [SerializeField]
    private float firstShootDelay;
    [SerializeField]
    private bool shootsContinuously = true;
    
    protected override IEnumerator AttackRoutine()
    {
        yield return new WaitForSecondsRealtime(firstShootDelay);
        shootContinuously = shootsContinuously;
        Shoot();
    }
}
