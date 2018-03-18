using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class PlayerTracking : MonoBehaviour {
    static int sphereCount = 0;
    int mySphereNumber = -1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (mySphereNumber < 0)
            mySphereNumber = sphereCount++;
        var targetConnection = NetworkServer.connections.Count == 0 || mySphereNumber == 0 ? NetworkServer.connections[0] : NetworkServer.connections[mySphereNumber % NetworkServer.connections.Count];
        if (targetConnection.playerControllers.Count == 0)
            return;
        var targetController = targetConnection.playerControllers[0];
        if (targetController == null)
            return;
        var playerGameObject = targetController.gameObject;
        if (playerGameObject != null)
        {
            Transform playerTransform = playerGameObject.transform;
            Vector3 playerPosition = playerTransform.position;
            GetComponent<NavMeshAgent>().SetDestination(playerPosition);
        }
    }
}