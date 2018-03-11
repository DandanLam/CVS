using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubits : MonoBehaviour, IPickUp{

	
    public void PickMeUp()
    {
        //do someothing here
        Debug.LogError("YOu PICKED ME UP NOO!");
        DestroyObject(this.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
       // Destroy(gameObject);
    }
}
