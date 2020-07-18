using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{

    Rigidbody2D rigidbody;
    public float bulletLifetime = 5.0f;
    public int bulletForce = 400;

    void Awake()
    {
       rigidbody = GetComponent<Rigidbody2D>(); 
    }

    public void Fire(int direction)
    {
        rigidbody.AddForce(new Vector2(direction * bulletForce, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletLifetime < 0)
        {
            Destroy(gameObject);
        }
        bulletLifetime -= Time.deltaTime;
    }
}
