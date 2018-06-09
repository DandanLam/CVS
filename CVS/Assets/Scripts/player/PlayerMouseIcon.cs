using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseIcon : MonoBehaviour {
    public List<GameObject> icons = new List<GameObject>();

    public void setIcon(PowerUpType type)
    {
        foreach (GameObject o in icons)
        {
            o.SetActive(false);
        }
        icons[(int)type].SetActive(true);
    }
}
