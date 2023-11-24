using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    public Transform target;
    Animator animator;
    NavMeshAgent agent;
    [SerializeField] float currentspeed;
    public bool alerted;
    public bool playerSpotted;
    public float runningSpeed;
    public float WalkingSpeed;
    public Collider[] bulletAlertColliders;

    [Header("Enemy Weapons")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate;
    [SerializeField] private float timeToShoot;
    

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemySpeedChange();
        agent = GetComponent<NavMeshAgent>();
        currentspeed = agent.velocity.magnitude;
        
        animator.SetFloat("Speed",currentspeed);
        if(playerSpotted)
        {
            animator.SetBool("Shooting" , true);
            Shooting();
        }
        else 
        {
            animator.SetBool("Shooting" , false);
        }
        
    }

    void EnemySpeedChange()
    {
        if(alerted)
        {
            agent.speed = runningSpeed;
        }
        else
        {
            agent.speed = Mathf.Lerp(agent.velocity.magnitude, WalkingSpeed,0.5f);
        }
    }
    void BulletAlertDetection()
    {
        foreach(var bulletAlertCollider in bulletAlertColliders)
        {
            if(bulletAlertCollider.GetComponent<EnemyAI>())
            {
                bulletAlertCollider.GetComponent<EnemyAI>().alerted = true;
            }
        }
    }

    void Shooting()
    {
        Vector3 targetpos = new Vector3(target.position.x,transform.position.y,target.position.z);
        Vector3 targetposFirePoint = new Vector3(target.position.x,target.position.y,target.position.z);
        transform.LookAt(targetpos);
        firePoint.transform.LookAt(targetposFirePoint);
        if(Time.time > timeToShoot)
        {
            timeToShoot = Time.time + fireRate;

            var bulletGO = Instantiate(bulletPrefab,firePoint.position,firePoint.rotation);
            var bullet = bulletGO.GetComponent<Bullet>();
            bullet.Fire(firePoint);
        }
    }
}
