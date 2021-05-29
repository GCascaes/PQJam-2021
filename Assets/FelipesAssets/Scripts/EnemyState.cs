using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void Log()
    {
        Debug.Log(gameObject.name + " is in " +  GetComponent<EnemyStateMachine>().GetCurrentState().ToString() + " State..");
    }

}
