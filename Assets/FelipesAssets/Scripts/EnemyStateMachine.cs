using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{ 

    [SerializeField]
    private EnemyStatesEnum currentState;

    private EnemyStatesEnum previousState;

    [SerializeField]
    private EnemyStatesEnum nextState;

    [SerializeField]
    private float stateMachineUpdateCounter = 3f;

    private EnemyIdle idle;

    private EnemyPatrol patrol;

    private EnemyAttack attack;

    //temporary
    [SerializeField]
    private bool isAlive;

    public enum EnemyStatesEnum {
        IDLE,
        PATROL,
        ATTACK
    }

    private void Awake()
    {

        idle = gameObject.AddComponent<EnemyIdle>();

        patrol  = gameObject.AddComponent<EnemyPatrol>();

        attack  = gameObject.AddComponent<EnemyAttack>();

        isAlive = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Adds Enemy Idle state as entry point for its stateMachine
        currentState = EnemyStatesEnum.IDLE;
        
        StartCoroutine(EnemyLogic());
    }

    public void SetCurrentState(EnemyStatesEnum newState)
    {
        previousState = currentState;

        nextState = newState;
    }

    public EnemyStatesEnum GetCurrentState()
    {
        return currentState;
    }

    private void NextState()
    {
        idle.enabled = false;

        patrol.enabled = false;

        attack.enabled = false;

        switch(currentState)
        {
            case EnemyStatesEnum.IDLE:
                idle.enabled = true;

                break;

            case EnemyStatesEnum.PATROL:
                patrol.enabled = true;

                break;

            case EnemyStatesEnum.ATTACK:
                attack.enabled = true;

                break;

            default:

                break;
        }
    }

    // Update is called once per frame
    IEnumerator EnemyLogic()
    {
        while(isAlive)
        {

            if(currentState != nextState)
            {
                currentState = nextState;

                NextState();
            }

            if(currentState == EnemyStatesEnum.IDLE)
            {
                nextState = EnemyStatesEnum.PATROL;
            }

            //If enemy state is changed to another state that is not Attack, the change is not immediate
            if (nextState != EnemyStatesEnum.ATTACK)
            {
                yield return new WaitForSecondsRealtime(stateMachineUpdateCounter);
            }

            yield return new WaitForSecondsRealtime(0);

        }

    }
}
