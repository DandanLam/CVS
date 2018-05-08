using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Collectible : NetworkBehaviour, IPickUp {

    
    public void PickMeUp()
    {
        Destroy(this.gameObject);
    }
    

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == StringConstants.playerTag)
        {
            PickMeUp();
        }
    }

}

