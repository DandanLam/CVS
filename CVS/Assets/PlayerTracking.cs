using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerTracking : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GameObject player = GameObject.Find("Player(Clone)");
        if (player != null)
        {
            Transform playerTransform = player.transform;
            Vector3 playerPosition = playerTransform.position;
            GetComponent<NavMeshAgent>().SetDestination(playerPosition);
        }
    }
}
