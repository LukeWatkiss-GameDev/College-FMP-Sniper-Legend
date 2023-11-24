using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Movement : MonoBehaviour
{
    public float speed = 5; 
    private float gravity = -9.81f; 
    private CharacterController controller; 
    private Vector3 velocity; 
    private Vector3 moveDirection; 

    void Start()
    {
        controller = GetComponent<CharacterController>(); 
    }

    void Update()
    {

        if(controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

    }
    
    void FixedUpdate()
    {
        Walking(); 
    }  

    void Walking()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal"); 
        float moveVertical = Input.GetAxisRaw("Vertical");   
        moveDirection = (moveHorizontal * transform.right + moveVertical * transform.forward); 
        controller.Move(moveDirection * speed * Time.deltaTime);  
        velocity.y += gravity * Time.deltaTime; 
        controller.Move(velocity * Time.deltaTime); 
    }

}
