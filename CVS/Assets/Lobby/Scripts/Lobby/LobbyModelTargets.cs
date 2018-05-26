using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyModelTargets : MonoBehaviour {

    public List<GameObject> ModelList = new List<GameObject>();

    private float locationOffset = 10f;

    public void AddNewModel(GameObject o){
        ModelList.Add(o);
        o.transform.position = new Vector3(transform.position.x + (locationOffset*ModelList.Count), transform.position.y, transform.position.z);
    }

    //TODO later, maybe for players to unlock
    public void PopulateModels()
    {

    }

}
