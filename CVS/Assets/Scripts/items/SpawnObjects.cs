using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class SpawnObjects : NetworkBehaviour {

    public GameObject prefab;

    public bool respawnEnabled;
    public float respawnTimerInSec;
    public bool spawnAtCornersOfPlane;
    public int spawnByNumber;

    private Vector3 planeScale;
    private Vector3 planeCenterPosition;
    private float timer;

    // Use this for initialization
    public override void OnStartServer()
    {
        if (!isServer)
            return;
        InitializeVariables();
        SpawnPrefabs(spawnByNumber);
    }
	
	// Update is called once per frame
	void Update ()
    {
        RespawnPrefabs();
	}

    public void RespawnPrefabs()
    {
        if (!respawnEnabled)
            return;

        timer += Time.deltaTime;

        if (timer > respawnTimerInSec) {
            int numberOfPrefabsToRespawn = GetNumberOfPrefabsToRespawn();

            SpawnPrefabs(numberOfPrefabsToRespawn);
            
            timer = 0f;
        }
    }

    public int GetNumberOfPrefabsToRespawn()
    {
        int existingGameObjectsCount = 0;

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("pickable")) {
            if (go.name == (prefab.name + "(Clone)")) {
                existingGameObjectsCount++;
            }
        }

        return spawnByNumber - existingGameObjectsCount;
    }

    public void InitializeVariables()
    {
        GameObject plane = GameObject.Find("Plane");

        Transform planeTransform = plane.transform;

        planeScale = planeTransform.localScale;
        planeCenterPosition = planeTransform.position;

        timer = 0f;
    }

    public void SpawnPrefabs(int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 positionOnPlane = getRandomPositionOnPlane();
            GameObject instance = Instantiate(prefab, TransferToPositionOnNavMesh(positionOnPlane), Quaternion.identity) as GameObject;
            NetworkServer.Spawn(instance);
        }
        
    }

    public Vector3 getRandomPositionOnPlane()
    {
        int y = 3;

        Vector3 newPosition = planeCenterPosition + new Vector3(Random.Range(-planeScale.x * planeScale.x, planeScale.x * planeScale.x), y, Random.Range(-planeScale.z * planeScale.z, planeScale.z * planeScale.z));

        if (spawnAtCornersOfPlane)
        {
            ArrayList cornerPositions = new ArrayList();

            cornerPositions.Add(new Vector3((-planeScale.x * planeScale.x), y, (-planeScale.z * planeScale.z)));
            cornerPositions.Add(new Vector3((-planeScale.x * planeScale.x), y, (planeScale.z * planeScale.z)));
            cornerPositions.Add(new Vector3((planeScale.x * planeScale.x), y, (-planeScale.z * planeScale.z)));
            cornerPositions.Add(new Vector3((planeScale.x * planeScale.x), y, (planeScale.z * planeScale.z)));

            newPosition = (Vector3)cornerPositions[Random.Range(0, 4)];
        }

        return newPosition;
    }

    public Vector3 TransferToPositionOnNavMesh(Vector3 position)
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(position, out hit, 5.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return position;
    }

    /*
    public float GetNumberOfSpawnObjectsByPlaneSize(float percentageRelativeToPlane) {
        return ((percentageRelativeToPlane / 100) * (planeScale.x * planeScale.x));
    }
    */
}
