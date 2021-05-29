using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySightArea : MonoBehaviour
{
    [SerializeField]
    private PolygonCollider2D sightArea;

    [SerializeField]
    private EnemyStateMachine stateMachine;


    //[SerializeField]
    //According to unity documentation, the range is non editable in runtime
    //private float enemyHorizontalMaxRange;

    // Start is called before the first frame update
    void Awake()
    {
        sightArea = GetComponent<PolygonCollider2D>();
        stateMachine = GetComponent<EnemyStateMachine>();
    
        //According to unity documentation, the range is non editable in runtime
        //sightArea.points[1] = new Vector2(enemyHorizontalMaxRange, sightArea.points[1].y);
        //sightArea.points[2] = new Vector2(enemyHorizontalMaxRange, sightArea.points[2].y);

        sightArea.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            stateMachine.SetCurrentState(EnemyStateMachine.EnemyStatesEnum.ATTACK);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            stateMachine.SetCurrentState(EnemyStateMachine.EnemyStatesEnum.IDLE);
        }
    }
}
