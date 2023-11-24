using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTimeCam : MonoBehaviour
{
    public Vector3 camPosition;
    public GameObject camPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(camPosition != new Vector3(0,0,0))
        {
            Instantiate(camPrefab,camPosition,transform.rotation);
            camPosition = new Vector3(0,0,0);
            
        }
    }
}
