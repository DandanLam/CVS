using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        var hit = other.gameObject;
        Debug.Log("other object: " + other.gameObject.name);
        if (hit.tag == "Player")
        {
            var health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(Health.maxHealth / 4);
            }
        }
    }
}
