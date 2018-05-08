using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {
    

    void OnTriggerEnter(Collider other)
    {
        var hit = other.gameObject;
        if (hit.tag == StringConstants.playerTag)
        {
            var health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(Health.maxHealth / 4);
            }
        }
    }
}
