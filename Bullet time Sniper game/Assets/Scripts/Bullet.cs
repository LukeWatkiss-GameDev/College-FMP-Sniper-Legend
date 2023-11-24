using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    public float bulletSpeed;
    public RaycastHit hit;
    public LayerMask mask;
    public float radius;
    public float gunDamage;
    Rifle rifle;
    public Camera cam;
    public Vector3 camEndPos;
    BulletTimeCam bulletTimeCam;
    [SerializeField] private GameObject alertedCollider;
    [SerializeField] private float SphereCastLength;
    Vector3 startForward;
    Vector3 startPosition;
    Vector3 velocity;
    float gravity;
    bool isInitialized;
    float startTime = -1;

    // the making of this scipt included some help from a fellow classmate
    public void Fire(Transform position)
    {

        rb = GetComponent<Rigidbody>();
        rifle = GameObject.Find("AWM_Sniper_Rifle").GetComponent<Rifle>();
        bulletTimeCam = GameObject.Find("Player").GetComponent<BulletTimeCam>();
        cam = gameObject.GetComponentInChildren<Camera>();

        startForward = position.forward;
        startPosition = position.position;

        gravity = 9.81f;
        isInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (!isInitialized || startTime < 0)
            return;

        float currentTime = Time.time - startTime;
        Vector3 currentPosition = FindPosition(currentTime);

        transform.position = currentPosition;
        velocity = (currentPosition - FindPosition(currentTime - Time.deltaTime)) / Time.deltaTime;
    }
    private Vector3 FindPosition(float time) 
    {
        Vector3 point = startPosition + startForward * bulletSpeed * time;
        Vector3 gravityVector = Vector3.down * gravity * time * time;
        return point + gravityVector;
    }

    private bool CastRayBetweenPoints(Vector3 start, Vector3 end, out RaycastHit hit)
    {
        return Physics.Raycast(start, end - start, out hit, (end - start).magnitude);
    }

    private void FixedUpdate()
    {
        if (!isInitialized)
            return;
        if (startTime < 0)
            startTime = Time.time;

        RaycastHit hit;
        float currentTime = Time.time - startTime;
        float prevTime = currentTime - Time.fixedDeltaTime;
        float nextTime = currentTime + Time.fixedDeltaTime;

        if (prevTime > 0)
        {
            Vector3 prevPoint = FindPosition(prevTime);
        }

        Vector3 currentPosition = FindPosition(currentTime);
        Vector3 prevPosition = FindPosition(prevTime);
        Vector3 nextPosition = FindPosition(nextTime);



        if (CastRayBetweenPoints(currentPosition, nextPosition, out hit))
        {
            Vector3 hitPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            if(alertedCollider != null)
            {
                Instantiate(alertedCollider,hitPos,Quaternion.identity);
            }
            
            
            Destroy(gameObject);
            rifle.hit = hit;
            Debug.Log(hit.transform.gameObject);
            Target target = hit.transform.GetComponent<Target>();
            var HitBox = hit.collider.GetComponent<HitBox>();
            if(hit.transform.GetComponent<ExplosiveObject>())
            {

                hit.transform.GetComponent<ExplosiveObject>().Explosion(30f,7.5f);
                
                camEndPos = cam.transform.position ;
                bulletTimeCam.camPosition = camEndPos;
            }
            if(HitBox)
            {
                HitBox.OnRaycastHit(this, transform.forward);
                if(CompareTag("Bullets"))
                {
                    
                    camEndPos = cam.transform.position ;
                    bulletTimeCam.camPosition = camEndPos;
                }
                
                
                
            }
            if(hit.transform.GetComponent<PlayerHealth>())
            {
                hit.transform.GetComponent<PlayerHealth>().health -= 10f;
            }
        }

    }
}
