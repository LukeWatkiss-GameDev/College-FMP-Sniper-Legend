using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour
{
    [SerializeField, Header("Pathfinding")]
    
    Transform[] waypoints;
    EnemyAI enemyAI;
    Animator animator;
    
    [SerializeField]bool loop;
    //check the loop bool if you want the Agent to loop around the points

    [SerializeField] int waypointCount, waypointIteration;
    NavMeshAgent agent;
    [SerializeField] float timer = 0;
    [SerializeField] float waitTime = 2;
    [SerializeField] bool WaitAtPoints; 
    public Transform bulletImpact;
    [SerializeField] bool debug;
    [Header("StaticEnemys")]
    [SerializeField] bool StaticEnemy;
    [SerializeField] GameObject staticPos;
    [SerializeField] Rot rotation = new Rot();
    bool rotate180,rotate90L,rotate90R,noRotation;    
    public List<Transform> seenBodies = new List<Transform>();
    public List<GameObject> enemies = new List<GameObject>();

    
    EnemyFOV enemyFovScript;
    enum Rot
    {
        rotate180,
        rotate90L,
        rotate90R,
        noRotation
    };

    void Start()
    {
        
        References();
    }

    void PathfindingAI()
    {
        
        if(seenBodies != null)
        {
            foreach(var transform in seenBodies)
            {
                for(int i = 0; i < enemies.Count; i++)
                {
                    
                    if(enemies[i].GetComponent<Pathfinding>().seenBodies.Contains(transform))
                    { 
                        agent.SetDestination(transform.position);
                        
                        Debug.Log(enemies[i].GetComponent<NavMeshAgent>().destination);
                        Debug.Log(enemies[i]); 
                        enemies[i].GetComponent<Pathfinding>();
                        
                        
                        
                        transform.GetComponent<Health>().bodyChecked = true;
                        seenBodies.Remove(transform);
                    }
                }
                
                
            }
        }
        
        
        if(bulletImpact != null)
        {
            agent.SetDestination(bulletImpact.position);
            bulletImpact = null;
        }

        if(waypoints[0] == null) return; 
        
        if (agent.remainingDistance < agent.stoppingDistance ) 
        {

            if(WaitAtPoints)
            {
                timer -= Time.deltaTime;
                if (timer < 0 )
                {   
                    NewDestination();
                    timer = waitTime;
                }
                

            }
            else
            {
                bulletImpact = null;
                NewDestination();
            }
            
        }
    }

    void FixedUpdate()
    {
        foreach(var transform in enemyFovScript.visableTargets)
        {
            if(!seenBodies.Contains(transform) && transform.GetComponent<EnemyAI>().enabled == false && transform.GetComponent<Health>().bodyChecked == false)
            {
                seenBodies.Add(transform);
            }
        }
        
    }
    void Update()
    {
         
        PathfindingAI();
            
        
        if(agent.destination != null && debug)
        {
           Debug.Log(name + " " + agent.destination);
        }
        
        
    }

    void References()
    {
        animator = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();
        enemies = new List<GameObject>( GameObject.FindGameObjectsWithTag("Enemy") );
        enemies.Remove(this.gameObject);
        enemyFovScript = GetComponent<EnemyFOV>();
        agent = GetComponent<NavMeshAgent>();
        timer = waitTime;
        if(StaticEnemy)
        {
            staticPos = new GameObject("soldierStartPos");
            staticPos.transform.position = transform.position;
           
            if(rotation == Rot.rotate180)
            {
                staticPos.transform.Rotate(0,180,0,Space.Self);
            }
            if(rotation == Rot.rotate90R)
            {
                staticPos.transform.Rotate(0,90,0,Space.Self);
            }
            if(rotation == Rot.rotate90L)
            {
                staticPos.transform.Rotate(0,-90,0,Space.Self);
            }

            waypoints[0] = staticPos.transform;
        }
        
        if(waypoints[0] == null) return; 

        //sets the AI to follow the waypoints when the scene starts 
        agent.SetDestination (waypoints [0].position);
        waypointCount = waypoints.Length;
        waypointIteration = 0;
    }


    void NewDestination()
    {
        if(waypointIteration < waypointCount) 
        { 
           
            agent.SetDestination(waypoints[waypointIteration].position);
            waypointIteration++; 
            
        }
        else
        {
            if(loop)
            {
                waypointIteration = 0;
            }
            if(StaticEnemy)
            {
                
                transform.rotation = Quaternion.Slerp(transform.rotation, waypoints[0].transform.rotation,.2f);    
                
                
            }
        }
    }


}