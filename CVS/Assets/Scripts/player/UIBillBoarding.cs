using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillBoarding : MonoBehaviour {

    // Update is called once per frame
    void Update() {
        if (Camera.main != null) { 
            transform.LookAt(Camera.main.transform);
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
