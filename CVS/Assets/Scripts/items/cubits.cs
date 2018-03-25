using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class cubits : NetworkBehaviour, IPickUp{

    public AudioClip shotSound;

    private void Awake()
    {
        if (shotSound != null && gameObject.tag == StringConstants.thorwableTag)
            AudioSource.PlayClipAtPoint(shotSound, new Vector3(gameObject.transform.position.x,
                                                               gameObject.transform.position.y + 10,
                                                               gameObject.transform.position.z));
    }

    public void PickMeUp()
    {
        //do someothing here
        DestroyObject(this.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        if (hit.tag == StringConstants.playerTag) { 
            var health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(-(Health.maxHealth / 4));
            }
        }else if(hit.tag == StringConstants.sphereTag)
        {
            var health = hit.GetComponent<Health>();
            if(health!= null)
            {
                health.TakeDamage(30);
            }
        }
        NetworkServer.Destroy(gameObject);
    }
}
