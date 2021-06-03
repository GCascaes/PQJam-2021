using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [SerializeField]
    private EnemyState initialState = EnemyState.NotEngaged;
    [SerializeField]
    private Collider2D seeTargetTriggerCollider;
    [SerializeField]
    private Collider2D followTargetTriggerCollider;
    [SerializeField]
    private Collider2D attackTargetTriggerCollider;
    [SerializeField]
    private Collider2D disengageTriggerCollider;
    [SerializeField]
    private float disengageTime;
    [SerializeField]
    private string targetTag;
    [SerializeField]
    private float gettingAngryTime;
    [SerializeField]
    private float gettingCuteTime;
    [SerializeField]
    private List<Collider2D> collidersToDisableWhenCute;

    private EnemyState currentState;

    public EnemyState CurrentState
    {
        get { return currentState; }
    }

    private Queue<EnemyState> stateChangeCommandQueue = new Queue<EnemyState>();
    private bool changingState = false;
    private bool isAngry;
    
    private GameObject currentTarget;

    private IPatrolMovementAi patrolMovementAi;
    private IFollowMovementAi followMovementAi;
    private IAttackAi attackAi;

    private Animator animator;

    private void Awake()
    {
        stateChangeCommandQueue.Enqueue(initialState);

        patrolMovementAi = GetComponent<IPatrolMovementAi>();
        followMovementAi = GetComponent<IFollowMovementAi>();
        attackAi = GetComponent<IAttackAi>();

        animator = GetComponent<Animator>();

        if (!isAngry)
        {
            foreach(var collider in collidersToDisableWhenCute)
                collider.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (!stateChangeCommandQueue.Any() || changingState)
            return;

        StartCoroutine(ChangeState(stateChangeCommandQueue.Dequeue()));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(targetTag))
            return;

        currentTarget = collision.gameObject;

        if (seeTargetTriggerCollider != null && collision.IsTouching(seeTargetTriggerCollider))
            stateChangeCommandQueue.Enqueue(EnemyState.SeenTarget);

        if (followTargetTriggerCollider != null && collision.IsTouching(followTargetTriggerCollider))
            stateChangeCommandQueue.Enqueue(EnemyState.FollowingTarget);

        if (attackTargetTriggerCollider != null && collision.IsTouching(attackTargetTriggerCollider))
            stateChangeCommandQueue.Enqueue(EnemyState.AttackingTarget);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(targetTag))
            return;

        if (disengageTriggerCollider != null && !collision.IsTouching(disengageTriggerCollider))
            stateChangeCommandQueue.Enqueue(EnemyState.Disengaged);
    }

    private IEnumerator ChangeState(EnemyState newState)
    {
        changingState = true;

        switch (newState)
        {
            case EnemyState.SeenTarget when currentState == EnemyState.NotEngaged:
            case EnemyState.SeenTarget when currentState == EnemyState.Disengaged:
                if (patrolMovementAi != null)
                    patrolMovementAi.StopPatroling();
                if (followMovementAi != null)
                    followMovementAi.LookAtTarget(currentTarget);
                yield return GetAngry();
                break;
            case EnemyState.FollowingTarget when currentState == EnemyState.SeenTarget:
                if (followMovementAi != null)
                    followMovementAi.FollowTarget(currentTarget);
                break;
            case EnemyState.AttackingTarget when currentState == EnemyState.SeenTarget:
            case EnemyState.AttackingTarget when currentState == EnemyState.FollowingTarget:
                if (attackAi != null)
                    attackAi.AttackTarget(currentTarget);
                break;
            case EnemyState.Disengaged:
                if (followMovementAi != null)
                    followMovementAi.StopFollowing();
                if (attackAi != null)
                    attackAi.StopAttacking();
                StartCoroutine(DisengageCooldown());
                break;
            case EnemyState.NotEngaged:
                yield return GetCute();
                if (patrolMovementAi != null)
                    patrolMovementAi.StartPatroling();
                break;
        }

        currentState = newState;
        changingState = false;
    }

    private IEnumerator GetAngry()
    {
        if (isAngry)
            yield break;

        if (animator != null)
            animator.SetBool("isAngry", true);

        foreach (var collider in collidersToDisableWhenCute)
            collider.enabled = true;

        isAngry = true;
        yield return new WaitForSeconds(gettingAngryTime);
    }

    private IEnumerator GetCute()
    {
        if (!isAngry)
            yield break;

        if (animator != null)
            animator.SetBool("isAngry", false);

        foreach (var collider in collidersToDisableWhenCute)
            collider.enabled = false;

        isAngry = false;
        yield return new WaitForSeconds(gettingCuteTime);
    }

    private IEnumerator DisengageCooldown()
    {
        yield return new WaitForSeconds(disengageTime);

        if (currentState == EnemyState.Disengaged)
            stateChangeCommandQueue.Enqueue(EnemyState.NotEngaged);
    }
}
