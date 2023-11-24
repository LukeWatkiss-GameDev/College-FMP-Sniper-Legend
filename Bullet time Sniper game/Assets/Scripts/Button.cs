using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    Transform top;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        top = transform.Find("top");
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
