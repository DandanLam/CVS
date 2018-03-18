using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        if (hit.tag == "Player")
        {
            var player = hit.GetComponent<Player>();
            player.IsFrozen = true;
            var health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(Health.maxHealth / 4);
            }
        }
    }
}
