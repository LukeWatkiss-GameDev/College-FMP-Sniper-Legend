using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Health health;
    // Start is called before the first frame update


    public void OnRaycastHit(Bullet bullet , Vector3 direction)
    {
        health.TakeDamage(bullet.gunDamage, direction);
    }
}
