using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Collectible : NetworkBehaviour, IPickUp {

    
    public void PickMeUp()
    {
        //do someothing here
        DestroyObject(this.gameObject);
    }


}

