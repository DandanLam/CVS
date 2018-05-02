using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBounce : MonoBehaviour {
    // Update is called once per frame
    Vector3 startPos;
    private void Start()
    {
        startPos = transform.position;
    }
    void FixedUpdate () {
		transform.localPosition = new Vector3 (transform.localPosition.x, Mathf.PingPong(Time.time/5, 0.2f)+ startPos.y, transform.localPosition.z);
	}
}
