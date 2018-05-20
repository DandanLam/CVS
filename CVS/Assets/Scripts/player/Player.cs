using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour{

    public PowerUpType currentPowerup;
    bool powerupIsActive = false;
    DateTime powerupActivationTime = new DateTime(0);
    public GameObject m_Prefab;
    float defaultrunSpeed = 5;
    float defaultwalkSpeed = 3;

    [SerializeField]
    private Health myHealthComponent;

    [SyncVar]
    public string name = "";

    [SyncVar]
    public bool IsFrozen = false;

    [SerializeField]
    private float tossRange = 5f;

    [SerializeField]
    private float distancefromPlayer = 1.1f;

    [SerializeField]
    private int cubitsNum;
    public int CubitsNum
    {
        get { return cubitsNum; }
        set
        {
            cubitsNum = value;
            GetComponent<PlayerHUD>().myCubitsNumberText.text = value+"";

        }
    }

    [SerializeField]
    private GameObject throwableCubePrefab;

    [SerializeField]
    private GameObject throwableBoostedCubePrefab;

    // Use this for initialization

    [SerializeField]
    private GameObject clientOnlyObjects;


    [SerializeField]
    private Text playerNameText;

	void Start () {
        playerNameText.text = name;
        currentPowerup = PowerUpType.NONE;
        if (!isLocalPlayer)
        {
            clientOnlyObjects.SetActive(false);
        }
	}

    void Update()
    {
        float runSpeed = 5;
        float walkSpeed = 3;
        if (!isLocalPlayer) { 
            return;
        }
        
        if (IsFrozen)
            return;

        powerupIsActive = powerupActivationTime.Add(TimeSpan.FromSeconds(10)) >= DateTime.Now ? true : false;

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var verticalAxis = Input.GetAxis("Vertical");
        if (verticalAxis < 0)
        {
            runSpeed  = 2.5f;
            walkSpeed = 2.5f;
        }
        
        if (powerupIsActive && currentPowerup == PowerUpType.SPEED)
        {
            runSpeed  = 2 * defaultrunSpeed;
            walkSpeed = 2 * defaultwalkSpeed;
        }

        var z = verticalAxis * Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed);

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        //left click do something here, 
        //TODO will change when VR implementation
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            
            if (CubitsNum <= 0)
            {
                return;
            }
            CubitsNum--;
                        
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (powerupIsActive && currentPowerup == PowerUpType.INVISIBLE)
                    CmdThrowCubeBoosted(hit.point);
                else
                    CmdThrowCube(hit.point);
            }            
        } else if (Input.GetMouseButtonDown(1))
        {
            // Activate Special Ability
            if (currentPowerup != PowerUpType.NONE && cubitsNum >= 5)
            {
                cubitsNum -= 5;
                powerupIsActive = true;
                powerupActivationTime = DateTime.Now;
            }
        }
    } 
    

    [Command]
    void CmdThrowCube(Vector3 location)
    {
        ThrowCube(location, throwableCubePrefab);
    }

    [Command]
    void CmdThrowCubeBoosted(Vector3 location)
    {
        ThrowCube(location, throwableBoostedCubePrefab);
    }

    void ThrowCube(Vector3 location, GameObject throwable)
    {
        Vector3 leveledLocation = new Vector3(location.x, transform.position.y, location.z);
        Vector3 targetVector = leveledLocation - transform.position;

        var leveledSpawnPoint = transform.position + targetVector.normalized * distancefromPlayer;
        GameObject cubeBall = Instantiate(throwable, leveledSpawnPoint, transform.rotation) as GameObject;

        cubeBall.GetComponent<Rigidbody>().velocity = targetVector.normalized * tossRange;
        NetworkServer.Spawn(cubeBall);
    }

    //Touch another object with .tag name
    private void OnTriggerEnter(Collider other)
    {
        if (isLocalPlayer && !powerupIsActive) {
            switch (other.tag)
            {
                default:
                case StringConstants.pickableTag:
                    CubitsNum++;
                    break;
                case StringConstants.powerupTag:
                    //TODO: Make random                   
                    currentPowerup = PowerUpType.DAMAGE;
                    break;
            }

            if (other.tag == StringConstants.sphereTag)
            {
                //Debug.LogError("Calling take damage");
                myHealthComponent.TakeDamage(Health.maxHealth / 4);
            }
        }
        
    }


    public void Dead()
    {
        Debug.Log("Updating Player froze variable");
        CmdDead();
        //freeze
    }

    [Command]
    void CmdDead()
    {
        IsFrozen = true;
    }

    public void Undead()
    {
        IsFrozen = false;
        //unfreeze
    }
}

public enum PowerUpType { NONE, SPEED, DAMAGE, INVISIBLE, BUILDER}