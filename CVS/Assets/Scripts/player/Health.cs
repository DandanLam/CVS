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
        OnChangeHealth(currentHealth);
    }
	
	// Update is called once per frame
	void FixedUpdate () {

    }



    public void TakeDamage(int amount)
    {
        if (!isServer)        
            return;
        
        currentHealth = amount > currentHealth ? 0 : currentHealth - amount;
        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;
        
    }

    void OnChangeHealth(int currentHealth)
    {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
        var parentObject = GetComponentInParent<Player>();
        parentObject.IsFrozen = currentHealth > 0 ? false : true;
    }
}
