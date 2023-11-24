using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    public Rigidbody[] rigidbodies;
    Animator animator;
    Rifle rifle;
    // Start is called before the first frame update
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        rifle = GameObject.FindWithTag("Rifle").GetComponent<Rifle>();
        DeactivateRagDoll();
    }

    // Update is called once per frame
    public void DeactivateRagDoll()
    {
        foreach(var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }
        animator.enabled = true;
        
    }
    public void ActivateRagDoll()
    {
        foreach(var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
            
        }
        animator.enabled = false;
    }

    public void ApplyForce(Vector3 force)
    {
        var rigidbody = rifle.hit.transform.GetComponent<Rigidbody>();
        rigidbody.AddForce(force, ForceMode.VelocityChange);
    }
}
