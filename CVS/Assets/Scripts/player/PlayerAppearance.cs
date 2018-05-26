using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearance : MonoBehaviour {

    public List<GameObject> ListOfApperances = new List<GameObject>();
    public GameObject activeApperance = null;

    public void SetApperance(int index)
    {
        foreach(GameObject o in ListOfApperances)
        {
            o.SetActive(false);
        }
        GameObject selectedObject = ListOfApperances[index];
        selectedObject.SetActive(true);
        activeApperance = selectedObject;
    }

    public void setColor(Color newColor)
    {
        if (activeApperance != null) { 
            activeApperance.GetComponent<Renderer>().material.color = newColor;
        }
    }

}
