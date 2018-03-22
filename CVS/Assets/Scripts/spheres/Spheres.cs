using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spheres : NetworkBehaviour{

	// Use this for initialization
	void Start () {
		
	}
	
    public void Dead()
    {
        NetworkServer.Destroy(gameObject);
        //actually dies, or disappears
    }

    public void Undead()
    {
    }
}
