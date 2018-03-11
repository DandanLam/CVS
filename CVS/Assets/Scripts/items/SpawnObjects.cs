using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour {

    public GameObject CubitPrefab;  //indicate this is a prefab and not an object in the game

    public float spawnPercentageRelativeToPlane; 
    private Vector3 planeCenterPosition;
    private Vector3 planeScale;

	// Use this for initialization
	void Start () {
        GameObject plane = GameObject.Find("Plane");

        Transform planeTransform = plane.transform;

        planeScale = planeTransform.localScale;
        planeCenterPosition = planeTransform.position;

        float numberOfSpawnObjects = (spawnPercentageRelativeToPlane / 100) * (planeScale.x * planeScale.x);

        for (int i=1; i < numberOfSpawnObjects; i++)
        {
            SpawnCubit();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKey("q"))
        {
            SpawnCubit();
        }
	}

    public void SpawnCubit()
    {
        Vector3 newPos = planeCenterPosition + new Vector3(Random.Range(-planeScale.x * planeScale.x, planeScale.x * planeScale.x), 0, Random.Range(-planeScale.z * planeScale.z, planeScale.z * planeScale.z));
        GameObject CubitInstance = Instantiate(CubitPrefab, newPos, Quaternion.identity) as GameObject; 
        //CubitInstance.GetComponent<Collider>().onTriggerEnter
            //attach script to ak-fighter adding a function onTriggerEnter() do stuff
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(planeCenterPosition, planeScale);
        //Gizmos.DrawCube(transform.localPosition + planeCenterPosition, planeScale);
    }
}
