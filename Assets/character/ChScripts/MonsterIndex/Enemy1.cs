using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    EnemyHit enemyHit;
    // Start is called before the first frame update
    void Start()
    {
        enemyHit = GetComponent<EnemyHit>();
        enemyHit.monsterNum = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
