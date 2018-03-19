using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class cubits : NetworkBehaviour, IPickUp{

	
    public void PickMeUp()
    {
        //do someothing here
        Debug.Log("Picked up cube!");
        DestroyObject(this.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        if (hit.tag == "Player") { 
            var health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(-(Health.maxHealth / 4));
            }
        }
        NetworkServer.Destroy(gameObject);
    }
}
