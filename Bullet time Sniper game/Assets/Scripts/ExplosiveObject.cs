using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveObject : MonoBehaviour
{
    [SerializeField] Collider[] hitColliders;
    [SerializeField] Collider[] ExplosionAlertColliders;
    BulletTime bulletTime;
    AudioSource audioSource;
    ParticleSystem explosion;
    

    // Start is called before the first frame update
    void Start()
    {
        explosion = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        bulletTime = GameObject.FindWithTag("Rifle").GetComponent<BulletTime>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == bulletTime.slowMotionTimeScale)
        {
            audioSource.pitch = .25f;
        }
        else
        {
            audioSource.pitch = 1;
        }
    }

    public void Explosion(float explosionForce, float radius)
    {
        audioSource.Play();
        explosion.Play();
        hitColliders = Physics.OverlapSphere(transform.position,radius);
        foreach(var hitCollider in hitColliders)
        {
            
            if(hitCollider.GetComponentInParent<Ragdoll>())
            {
                
                hitCollider.GetComponentInParent<Ragdoll>().ActivateRagDoll();
            }
            if(hitCollider.GetComponent<Rigidbody>())
            {
                
                
                
                Vector3 direction = transform.position - hitCollider.transform.position; 
                hitCollider.attachedRigidbody.AddExplosionForce(explosionForce,transform.position,radius,0f,ForceMode.Impulse); 
            }
            if(hitCollider.GetComponentInParent<Health>())
            {
                hitCollider.GetComponentInParent<Health>().currentHealth = 0;
            }
            
            
            
        }
        ExplosionsAlertdetection();
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject,5f);
    }

    void ExplosionsAlertdetection()
    {
        ExplosionAlertColliders = Physics.OverlapSphere(transform.position,30);
        foreach(var alertCollider in ExplosionAlertColliders)
        {
            if(alertCollider.GetComponent<EnemyAI>())
            {
                alertCollider.GetComponent<EnemyAI>().alerted = true;
            }
        }
    }

    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,7.5f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,30);
    }

    
}
