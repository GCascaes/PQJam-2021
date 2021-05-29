using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : EnemyStates
{
    //Can be used if any angry animations might be reset to 'tender' face animation (irritado -> fofinho)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InvokeRepeating("Log", 1, 5);
    }

}
