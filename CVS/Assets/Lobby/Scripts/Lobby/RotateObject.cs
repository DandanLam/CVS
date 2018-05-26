using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {
    Vector3 rotationDirection = new Vector3(0,1f, 0);
	void FixedUpdate () {
        transform.Rotate(rotationDirection);
	}
}
