using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBounce : MonoBehaviour {	
	// Update is called once per frame
	void FixedUpdate () {
		transform.localPosition = new Vector3 (transform.localPosition.x, Mathf.PingPong(Time.time/5, 0.2f)+0.1f, transform.localPosition.z);
	}
}
