using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAlerts : MonoBehaviour
{
    [SerializeField] Collider[] enemyAI;
    // Start is called before the first frame update
    void Start()
    {
        enemyAI = Physics.OverlapSphere(transform.position,7.5f);
        foreach(var enemy in enemyAI)
        {
                
            if(enemy.GetComponent<EnemyAI>())
            {
                enemy.GetComponent<Pathfinding>().bulletImpact = transform;
                enemy.GetComponent<EnemyAI>().alerted = true;
                Debug.Log("enemy alerted");
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 7.5f);
    }
}
