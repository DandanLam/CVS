using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    public const int maxHealth = 100;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    //public bool IsDead = { get { return currentHealth <= 0 ? true : false; } }

    
    public RectTransform healthBar;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        OnChangeHealth(currentHealth);

    }



    public void TakeDamage(int amount)
    {
        if (!isServer)        
            return;
        
        currentHealth -= amount;
        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead!");
            GetComponentInParent<Player>().IsFrozen = true;
        }
    }

    void OnChangeHealth(int currentHealth)
    {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
        if (currentHealth > 0)
        {
            var parentObject = GetComponentInParent<Player>();
            if (parentObject.IsFrozen)
                parentObject.IsFrozen = false;
        }
    }
}
