using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rifle : MonoBehaviour
{
    [Header("Weapon Stats")]
    public float gunDamage;
    public float gunRange;
    public float fireRate;
    public Transform firePoint;
    public GameObject rayPoint;

    private float nextFire;
    public int impactForce = 100;
    public float reloadTime;
    public int magSize;
    public int currentAmmo;
    public int reserveAmmo;
    public GameObject bulletPrefab;
    public GameObject bulletPrefab2;
    //public Text ammo;
    [Header("Recoil")]
    public float maxRecoil_X;
    public float currentRecoil;
    
    public float recoilIntensity;
    [Header("Scope")]
    public bool scopedIn;
    private Vector3 scopedInPos;
    [SerializeField]
    private Vector3 gunStartPos;
    public LayerMask shootingMask;
    private Camera scopeCam;
    public int[] scopeZooms;
    public float[] zeroDistances;
    private float t;
    int z;
    [SerializeField] int currentZoom = 0;
    [SerializeField] int currentZero = 0;
    [SerializeField] int[] currentZeroText;
    int ZeroText = 100;
    private Menu menuscript;
    public RaycastHit hit;
    public int randomNumber;
    BulletTime bulletTime;
    public FPS_Movement movementScript;
    public FPS_Camera cameraScript;
    Text zeoringText;
    [Header("Audio")]
    [SerializeField] AudioClip[] gunShot;
    [SerializeField] AudioClip reload;
    [SerializeField] AudioClip bolt;
    [SerializeField] AudioSource audioSource;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        zeoringText = GameObject.Find("Zeroing").GetComponent<Text>();
        bulletTime = GetComponent<BulletTime>();
        currentAmmo = magSize;
        maxRecoil_X = -5f;
        scopeCam = GameObject.Find("Scope Camera").GetComponent<Camera>();
        menuscript = GameObject.Find("Menu").GetComponent<Menu>();
        cameraScript = gameObject.GetComponentInParent<FPS_Camera>();
        movementScript = gameObject.GetComponentInParent<FPS_Movement>();

        gunStartPos = transform.localPosition;

        scopedInPos = new Vector3(0 ,-0.148f ,0.01f);
    }


    // Update is called once per frame
    void Update()
    {
        Scope();
        ScopeZoom();
        ZeroDistance();
        
        if(Time.timeScale == bulletTime.slowMotionTimeScale)
        {
            audioSource.pitch = .25f;
        }
        else
        {
            audioSource.pitch = 1;
        }

        if(Input.GetButton("Fire1")  && Time.time > nextFire && currentAmmo > 0 & menuscript.isPaused == false)
        {
            randomNumber = Random.Range(1,1);
            GunShoot();
            currentAmmo--;
            // recoil 
            
            currentRecoil += .1f;
            
        }
        if(currentAmmo < magSize)
        {
            if(currentAmmo <= 0 && reserveAmmo > 0)
            {
                Reload();
            }
            if(Input.GetKeyDown(KeyCode.R))
            {
                reserveAmmo = reserveAmmo + currentAmmo;
                currentAmmo = 0;
            }

        }
        
        Recoil();
    }

    void GunShoot()
    {
        nextFire = Time.time + fireRate;
        audioSource.PlayOneShot(gunShot[Random.Range(0,gunShot.Length)]);
        
        
        if(Physics.Raycast(rayPoint.transform.position, rayPoint.transform.forward , out hit , 1600f,shootingMask))
        {
            var bulletGO = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
            var bullet = bulletGO.GetComponent<Bullet>();
            bullet.Fire(firePoint);
            Debug.Log(hit.transform.gameObject);
            Target target = hit.transform.GetComponent<Target>();
            var HitBox = hit.collider.GetComponent<HitBox>();
            if(HitBox)
            {
                cameraScript.enabled = false;
                movementScript.enabled = false;
                bulletTime.SlowMotionOn();
            }
        }
        else
        {
            var bulletGO = Instantiate(bulletPrefab2, firePoint.transform.position, firePoint.transform.rotation) as GameObject;
            var bullet = bulletGO.GetComponent<Bullet>();
            bullet.Fire(firePoint);
        }
        audioSource.PlayOneShot(bolt);
    }

    void Reload()
    {
        nextFire = Time.time + reloadTime;
        
        if(reserveAmmo > magSize)
        {
            audioSource.PlayOneShot(reload);
            reserveAmmo = reserveAmmo - magSize;
            currentAmmo = magSize;
        }
        else
        {
            audioSource.PlayOneShot(reload);
            currentAmmo = reserveAmmo;
            reserveAmmo = 0;
        }
    }

    void Recoil()
    {
        
   
        if(currentRecoil > 0)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Lerp(transform.localPosition.z, transform.localPosition.z - .01f, 0.5f));
            Quaternion maxRecoil = Quaternion.Euler(maxRecoil_X, 0, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, maxRecoil, recoilIntensity); // rotates the weapon from its current position towards the max recoil value
            currentRecoil -= Time.deltaTime;    
            
        }
        else if(currentRecoil <= 0)
        {
            currentRecoil = 0;
            Quaternion minRecoil = Quaternion.Euler(0, transform.localRotation.y, transform.localRotation.z);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, minRecoil, recoilIntensity * 5 * Time.deltaTime); // rotates the weapon from its current rotation back to the original rotation
            transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y, Mathf.Lerp(transform.localPosition.z, gunStartPos.z, .1f));
        }
        

    }

    void Scope()
    {
        if(Input.GetButton("Fire2"))
        {
            if(t < 1)
            {
                t += 5f * Time.deltaTime;   
            }
            
            scopedIn = true;
            transform.localPosition = Vector3.Lerp(transform.localPosition, scopedInPos, t); 

        }
        else if(transform.localPosition != gunStartPos)
        {
            t = 0;
            t += 5f * Time.deltaTime;
            scopedIn = false;
            transform.localPosition = Vector3.Lerp(transform.localPosition, gunStartPos, t);
            currentZoom = 0;
        }

        if(scopedIn)
        {
            recoilIntensity = .25f;
            maxRecoil_X = -1f;
        }
        else if(!scopedIn)
        {
            recoilIntensity = .75f;
            maxRecoil_X = -5f;
        }


    }

    void ScopeZoom()
    {
        

        if(Input.GetKeyDown(KeyCode.LeftControl) && scopedIn)
        {
            if(currentZoom >= scopeZooms.Length -1)
            {
                currentZoom = 0;
                
                
            }
            else
            {
                currentZoom++;
                
            }
        }
        
        scopeCam.fieldOfView = Mathf.Lerp(scopeCam.fieldOfView, scopeZooms[currentZoom], 0.1f);
    }

    void ZeroDistance()
    {
        
        if(Input.GetKeyDown(KeyCode.Equals))
        {
            if(currentZero >= zeroDistances.Length -1)
            {
                currentZero = 0;
                z = 0;
                ZeroText = currentZeroText[z];
                
            }
            else
            {
                z++;
                currentZero++;
                ZeroText = currentZeroText[z];
            }
           
        }
        zeoringText.text = "Zeroing: " + ZeroText.ToString();
        scopeCam.transform.localRotation = Quaternion.Euler(zeroDistances[currentZero],scopeCam.transform.rotation.y,scopeCam.transform.rotation.z);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawRay(firePoint.transform.position, transform.forward * 1000);
    }
    

}
