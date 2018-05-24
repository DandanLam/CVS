using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Throwable : NetworkBehaviour{

    public int DamagePoints;
    public AudioClip shotSound;
    Rigidbody myRigidbody;
    Vector3 oldVel;
    DateTimeOffset spawnTime;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        spawnTime = DateTimeOffset.Now;
    }

    void FixedUpdate()
    {
        oldVel = myRigidbody.velocity;
        if (spawnTime.AddSeconds(5) <= DateTimeOffset.Now)
            NetworkServer.Destroy(gameObject);
    }

    private void Awake()
    {
        if (shotSound != null && gameObject.tag == StringConstants.thorwableTag)
        {
            GetComponent<AudioSource>().PlayOneShot(shotSound, 1f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        
            var hit = collision.gameObject;
            if (hit.tag == StringConstants.playerTag)
            {
                if (isServer)
                {
                    var health = hit.GetComponent<Health>();
                    if (health != null)
                    {
                        health.TakeDamage(-(DamagePoints));
                    }
                    NetworkServer.Destroy(gameObject);
                }
            }
            else if (hit.tag == StringConstants.sphereTag)
            {
                if (isServer)
                {
                    var health = hit.GetComponent<Health>();
                    if (health != null)
                    {
                        health.TakeDamage((DamagePoints));
                    }
                    NetworkServer.Destroy(gameObject);
                }
            }
            else
            {
                ContactPoint cp = collision.contacts[0];
                // calculate with addition of normal vector
                // myRigidbody.velocity = oldVel + cp.normal*2.0f*oldVel.magnitude;

                // calculate with Vector3.Reflect
                myRigidbody.velocity = Vector3.Reflect(oldVel, cp.normal);

                // bumper effect to speed up ball
                myRigidbody.velocity += cp.normal * 2.0f;
            }
        }
    
}