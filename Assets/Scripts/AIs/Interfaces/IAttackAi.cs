using UnityEngine;

public interface IAttackAi
{
    void AttackTarget(GameObject target);
    void StopAttacking();
}
