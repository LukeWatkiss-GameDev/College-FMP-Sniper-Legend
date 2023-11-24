using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFOV : MonoBehaviour
{
    public float enemyRange;
    [Range(0,360)]
    public float fovAngle;
    
    public LayerMask targetMask;
    public LayerMask obsticleMask;

    public List<Transform> visableTargets = new List<Transform>();
    [SerializeField] private GameObject playerOBJ;
    [SerializeField] private float timeToSpot;
    [SerializeField] private float timeSeen;
    EnemyAI enemyAI;
    public Image spottedImage;
    Transform player;
    

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        enemyAI = GetComponent<EnemyAI>();
        StartCoroutine("FindTargetsWithDelay", .2f);
    }


    void Update()
    {
        if(playerOBJ != null)
        {
            timeSeen += Time.deltaTime;
        }
        else if(timeSeen > 0)
        {
            timeSeen -= Time.deltaTime;
        }
        if(playerOBJ != null && timeSeen > timeToSpot)
        {

            enemyAI.alerted = true;
            enemyAI.playerSpotted = true;
            
        }
        else
        {
            enemyAI.playerSpotted = false;
        }
        spottedImage.transform.LookAt(player);
        spottedImage.color = new Color(255f,0f,0f,timeSeen/timeToSpot);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            FindVisableTargets();
        }
    }   
    
    void FindVisableTargets()
    {
        playerOBJ = null;
        visableTargets.Clear(); // clears the list to avoid duplicates 
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position,enemyRange, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward , dirToTarget) < fovAngle / 2)
            {
                float dstToTarget = Vector3.Distance (transform.position, target.position);

                if(!Physics.Raycast (transform.position, dirToTarget ,dstToTarget ,obsticleMask))
                {
                    if(targetsInViewRadius[i].CompareTag("Enemy"))
                    {
                        visableTargets.Add(target);
                    }
                    
                    if(targetsInViewRadius[i].CompareTag("Player"))
                    {
                        playerOBJ = targetsInViewRadius[i].gameObject;
                    }
                }
            }

        }
    }


    

    


    

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
