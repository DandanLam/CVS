using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class PlayerTracking : NetworkBehaviour {
    static int sphereCount = 0;
    int mySphereNumber = -1;
    
    public Transform target;

    [SyncVar]
    private Vector3 targetPosition = new Vector3();
    
	// Update is called once per frame
	void FixedUpdate () {
        GetComponent<NavMeshAgent>().SetDestination(targetPosition);

        if (!isServer) { return; }

        if (mySphereNumber < 0)
            mySphereNumber = sphereCount++;

        var activePlayers = GetActivePlayerGameObjects();
        if (activePlayers.Count == 0)
        {
            GetComponent<NavMeshAgent>().SetDestination(gameObject.transform.position);
            return;
        }

        //var playerGameObject = activePlayers[CalcPlayerToFollow(activePlayers.Count)];
        var playerGameObject = CalcPlayerToFollow(activePlayers);
        if (playerGameObject != null)
        {
            Transform playerTransform = playerGameObject.transform;
            target = playerTransform;
            targetPosition = playerTransform.position;
        }
    }

    GameObject CalcPlayerToFollow(List<GameObject> activePlayers)
    {
        float shortestDist = float.MaxValue;
        GameObject nearestPlayer = null;
        foreach (var activePlayer in activePlayers)
        {
            var dist = Vector3.Distance(activePlayer.transform.position, transform.position);
            if (nearestPlayer == null || dist < shortestDist)
            {
                var a = activePlayer.GetComponent<Player>().currentPowerup;
                var b = activePlayer.GetComponent<Player>().powerupIsActive;
                if (!(activePlayer.GetComponent<Player>().currentPowerup == PowerUpType.INVISIBLE && activePlayer.GetComponent<Player>().powerupIsActive))
                {
                    shortestDist = dist;
                    nearestPlayer = activePlayer;
                }
            }
        }
        return nearestPlayer;
    }

    //int CalcPlayerToFollow(int activePlayers)
    //{
    //    if (activePlayers == 0)
    //        return -1;
    //    else if (mySphereNumber == 0)
    //        return 0;
    //    else
    //        return mySphereNumber % activePlayers;
    //}

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