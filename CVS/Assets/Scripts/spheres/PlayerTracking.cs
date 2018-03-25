using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class PlayerTracking : MonoBehaviour {
    static int sphereCount = 0;
    int mySphereNumber = -1;
    public Transform target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (mySphereNumber < 0)
            mySphereNumber = sphereCount++;

        var activePlayers = GetActivePlayerGameObjects();
        if (activePlayers.Count == 0)
            return;
        var playerGameObject = activePlayers[CalcPlayerToFollow(activePlayers.Count)];
        if (playerGameObject != null)
        {
            Transform playerTransform = playerGameObject.transform;
            target = playerTransform;
            Vector3 playerPosition = playerTransform.position;
            GetComponent<NavMeshAgent>().SetDestination(playerPosition);
        }
        else
            GetComponent<NavMeshAgent>().destination = gameObject.transform.position;

    }

    int CalcPlayerToFollow(int activePlayers)
    {
        if (activePlayers == 0)
            return -1;
        else if (mySphereNumber == 0)
            return 0;
        else
            return mySphereNumber % activePlayers;
    }

    List<GameObject> GetActivePlayerGameObjects()
    {
        var activePlayerConnections = new List<GameObject>();
        if (NetworkServer.connections.Count > 0)
        {
            foreach (var connection in NetworkServer.connections)
            {
                try
                {
                    var gameObj = connection.playerControllers[0].gameObject;
                    if (!gameObj.GetComponent<Player>().IsFrozen)
                        activePlayerConnections.Add(gameObj);
                }
                catch { }
            }
        }
        return activePlayerConnections;
    }
}