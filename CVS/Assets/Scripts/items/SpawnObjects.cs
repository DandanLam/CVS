using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnObjects : NetworkBehaviour {

    public GameObject prefab;  //indicate this is a prefab and not an object in the game

    public int spawnByNumber; 

    private Vector3 planeCenterPosition;
    private Vector3 planeScale;

    // Use this for initialization
    public override void OnStartServer() {
        if (!isServer)
            return;
        GameObject plane = GameObject.Find("Plane");

        Transform planeTransform = plane.transform;

        planeScale = planeTransform.localScale;
        planeCenterPosition = planeTransform.position;

        //float numberOfSpawnObjects = (spawnByPercentageRelativeToPlane / 100) * (planeScale.x * planeScale.x);

        for (int i=1; i < spawnByNumber; i++)
        {
            SpawnPrefab();
        }
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void SpawnPrefab()
    {
        Vector3 newPos = planeCenterPosition + new Vector3(Random.Range(-planeScale.x * planeScale.x, planeScale.x * planeScale.x), 0, Random.Range(-planeScale.z * planeScale.z, planeScale.z * planeScale.z));
        GameObject instance = Instantiate(prefab, newPos, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(instance);
        
    }
}
