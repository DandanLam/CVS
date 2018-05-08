using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    public const int maxHealth = 100;
    public AudioClip hurtSound;
    public AudioClip deadSound;
    private AudioSource audioSource;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    bool WasLastHealthZero = false;
    public RectTransform healthBar;
    // Use this for initialization
    void Start () {
        OnChangeHealth(currentHealth);
    }


    public void TakeDamage(int amount)
    {
        if (isServer) { 
            currentHealth = amount > currentHealth ? 0 : currentHealth - amount;
            if (currentHealth >= maxHealth)
                currentHealth = maxHealth;

            if (hurtSound != null && amount > 0)
                audioSource.PlayOneShot(hurtSound);
            if (currentHealth > 0 && WasLastHealthZero)
            {
                WasLastHealthZero = false;
                GetComponent<Player>().Undead();
            }
            if (currentHealth == 0)
            {
                if (deadSound != null && !WasLastHealthZero)
                    audioSource.PlayOneShot(deadSound);
                WasLastHealthZero = true;
                if (gameObject.CompareTag(StringConstants.playerTag))
                {
                    Debug.Log("calling dead player");
                    GetComponent<Player>().Dead();
                }
                if (gameObject.CompareTag(StringConstants.sphereTag))
                {
                    GetComponent<Spheres>().Dead();
                }
            }
        }
        else
        {
            CmdTakeDamage(amount);
        }
    }
    
    [Command]
    void CmdTakeDamage(int amount)
    {
        currentHealth = amount > currentHealth ? 0 : currentHealth - amount;
        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;

        if (hurtSound != null && amount > 0)
            audioSource.PlayOneShot(hurtSound);
        if (currentHealth > 0 && WasLastHealthZero)
        {
            WasLastHealthZero = false;
            GetComponent<Player>().Undead();
        }
        if (currentHealth == 0)
        {
            if (deadSound != null && !WasLastHealthZero)
                audioSource.PlayOneShot(deadSound);
            WasLastHealthZero = true;
            if (gameObject.CompareTag(StringConstants.playerTag))
            {
                GetComponent<Player>().Dead();
            }
            if (gameObject.CompareTag(StringConstants.sphereTag))
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
