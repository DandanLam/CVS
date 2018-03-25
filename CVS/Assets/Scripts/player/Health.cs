using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    public const int maxHealth = 100;
    public AudioClip hurtSound;
    public AudioClip deadSound;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    //public bool IsDead = { get { return currentHealth <= 0 ? true : false; } }

    bool WasLastHealthZero = false;
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

        if (hurtSound != null && amount > 0)
            AudioSource.PlayClipAtPoint(hurtSound, new Vector3(gameObject.transform.position.x,
                                                               gameObject.transform.position.y + 10,
                                                               gameObject.transform.position.z));

        if (currentHealth > 0 && WasLastHealthZero){
                WasLastHealthZero = false;
                GetComponent<Player>().Undead();
        }
        if(currentHealth == 0){
            if (deadSound != null && !WasLastHealthZero)
                AudioSource.PlayClipAtPoint(deadSound, new Vector3(gameObject.transform.position.x,
                                                                   gameObject.transform.position.y + 10,
                                                                   gameObject.transform.position.z));
            WasLastHealthZero = true;
            if (gameObject.tag == "Player")
            {
                GetComponent<Player>().Dead();

            }
            if(gameObject.tag == "Sphere")
            {
                GetComponent<Spheres>().Dead();

            }
        }

    }

    void OnChangeHealth(int currentHealth)
    {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);

       
    }

}
