using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class SpawnObjects : NetworkBehaviour {

    public GameObject prefab;

    public int spawnByNumber;
    public bool respawnEnabled;
    public float respawnTimerInSec;

    private Vector3 planeScale;
    private Vector3 planeCenterPosition;
    private float timer;
    private float range = 3f;

    // Use this for initialization
    public override void OnStartServer() {
        if (!isServer)
            return;
        InitializeVariables();
        SpawnPrefabs(spawnByNumber);

        Vector3 point;

        if (RandomPoint(transform.position, range, out point))
        {
            Debug.DrawRay(point, Vector3.up, Color.red, 1.0f);
        }
    }
	
	// Update is called once per frame
	void Update () {
        RespawnPrefabs();
	}

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;

                return true;
            }
        }

        result = Vector3.zero;

        return false;
    }

    public void RespawnPrefabs() {
        if (!respawnEnabled)
            return;

        timer += Time.deltaTime;

        if (timer > respawnTimerInSec) {
            int numberOfPrefabsToRespawn = GetNumberOfPrefabsToRespawn();

            SpawnPrefabs(numberOfPrefabsToRespawn);
            
            timer = 0f;

            Debug.Log("Respawning " + numberOfPrefabsToRespawn + " " + prefab.name);
        }
    }

    public int GetNumberOfPrefabsToRespawn() {
        int existingGameObjectsCount = 0;

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("pickable")) {
            if (go.name == (prefab.name + "(Clone)")) {
                existingGameObjectsCount++;
            }
        }

        return spawnByNumber - existingGameObjectsCount;
    }

    public void InitializeVariables() {
        GameObject plane = GameObject.Find("Plane");

        Transform planeTransform = plane.transform;

        planeScale = planeTransform.localScale;
        planeCenterPosition = planeTransform.position;

        timer = 0f;
    }

    public void SpawnPrefab()
    {
        Vector3 newPos = planeCenterPosition + new Vector3(Random.Range(-planeScale.x * planeScale.x, planeScale.x * planeScale.x), 0, Random.Range(-planeScale.z * planeScale.z, planeScale.z * planeScale.z));
        GameObject instance = Instantiate(prefab, newPos, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(instance);
    }

    public void SpawnPrefabs(int spawnCount) {
        for (int i = 0; i < spawnCount; i++)
            SpawnPrefab();
    }

    /*
    public float GetNumberOfSpawnObjectsByPlaneSize(float percentageRelativeToPlane) {
        return ((percentageRelativeToPlane / 100) * (planeScale.x * planeScale.x));
    }
    */
}
