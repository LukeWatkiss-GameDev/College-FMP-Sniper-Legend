using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTime : MonoBehaviour
{
    public float slowMotionTimeScale;

    public float startTimeScale;
    public float startFixedDeltaTime;
    [SerializeField] Rifle rifle;
    // Start is called before the first frame update
    void Start()
    {
        startTimeScale = 1;
        startFixedDeltaTime = Time.fixedDeltaTime;
        rifle = gameObject.GetComponent<Rifle>();
    }

    // Update is called once per frame
    void Update()
    {
        HitDetection();
    }

    void HitDetection()
    {
        if(rifle.hit.transform != null && rifle.hit.transform.gameObject.GetComponent<ExplosiveObject>() != null)
        {
            SlowMotionOn();
            rifle.hit = new RaycastHit();
        }
        if(rifle.hit.transform != null &&  rifle.hit.transform.gameObject.GetComponent<HitBox>() != null && rifle.hit.transform.gameObject.GetComponentInParent<Health>().currentHealth >= 0)
        {
            if(rifle.randomNumber == 1)
            {
                SlowMotionOn();
                rifle.hit = new RaycastHit();
            }
            
            
        }
        else
        {
            rifle.hit = new RaycastHit();
        }
        
    }

    public void SlowMotionOn()
    {
        Time.timeScale = slowMotionTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime * slowMotionTimeScale;
        StartCoroutine(SlowMoOff(2f));
    }

    public void SlowMotionOff()
    {
        Time.timeScale = startTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime;
        StopCoroutine("SlowMoOff");
        rifle.randomNumber = -1;
        rifle.movementScript.enabled = true;
        rifle.cameraScript.enabled = true;
        
        
    }

    private IEnumerator SlowMoOff(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        SlowMotionOff();
    }

}
