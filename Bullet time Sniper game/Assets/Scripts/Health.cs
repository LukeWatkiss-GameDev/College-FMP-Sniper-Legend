using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float ragdollForce;
    public float currentHealth;
    Ragdoll ragdoll;
    [SerializeField] private GameObject enemyRifle;
    public bool bodyChecked;
    

    // Start is called before the first frame update
    void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
        currentHealth = maxHealth;

        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach(var rigidbody in rigidbodies)
        {
            HitBox hitBox = rigidbody.gameObject.AddComponent<HitBox>();
            hitBox.health = this;
        }
    }

    void Update()
    {
        if(currentHealth <= 0.0f)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            gameObject.GetComponent<EnemyAI>().enabled = false;
            gameObject.GetComponent<Pathfinding>().enabled = false;
            gameObject.GetComponent<EnemyFOV>().spottedImage.enabled = false;
            gameObject.GetComponent<EnemyFOV>().enabled = false;
            enemyRifle.transform.parent = null;
        }
    }

    // Update is called once per frame
    public void TakeDamage(float amount, Vector3 direction)
    {
        currentHealth -= amount;
        if(currentHealth <= 0.0f)
        {
            Die(direction);
        }
    }
    public void Die(Vector3 direction)
    {
        
        ragdoll.ActivateRagDoll();
        ragdoll.ApplyForce(direction * ragdollForce);
    }
}
