using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour{
    public PowerUpType currentPowerup;
    public bool powerupIsActive { get; private set; }
    DateTime powerupActivationTime = new DateTime();
    public GameObject m_Prefab;
    float defaultrunSpeed = 5;
    float defaultwalkSpeed = 3;

    [SyncVar(hook = "OnMyName")]
    public string playerName = "";
    [SyncVar(hook = "OnCharacterSelect")]
    public int characterSel = 0;
    [SyncVar(hook = "OnFrozen")]
    public bool IsFrozen = false;

    //Accessable references within Unity Editor
    [SerializeField]
    private float tossRange = 5f;
    [SerializeField]
    private Health myHealthComponent;
    [SerializeField]
    private float distancefromPlayer = 1.1f;
    [SerializeField]
    private GameObject throwableCubePrefab;
    [SerializeField]
    private GameObject throwableBoostedCubePrefab;
    [SerializeField]
    private PlayerAppearance playerAppearance;
    [SerializeField]
    private GameObject clientOnlyObjects;
    [SerializeField]
    private Text playerNameText;
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

	void Start () {
        currentPowerup = PowerUpType.NONE;
        //var renderer = gameObject.GetComponent<Renderer>();
        //renderer.material.color = Color.white;
        playerAppearance.setColor(Color.white);
        if (isLocalPlayer)
        {
            SetupLocalPlayer();
        }
        else
        {
            SetupOtherPlayer();
        }

        OnMyName(playerName);
        OnCharacterSelect(characterSel);
    }

    void SetupLocalPlayer()
    {
        clientOnlyObjects.SetActive(true);

    }
    void SetupOtherPlayer()
    {
        clientOnlyObjects.SetActive(false);

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
        if (!powerupIsActive)
        {
            //var renderer = gameObject.GetComponent<Renderer>();
            //renderer.material.color = Color.white;

            playerAppearance.setColor(Color.white); // this will affect the material off all objects that have it on scene.
        }

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
                if (powerupIsActive && currentPowerup == PowerUpType.DAMAGE)
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
                var renderer = gameObject.GetComponent<Renderer>();
                switch (currentPowerup)
                {
                    case PowerUpType.SPEED:
                        renderer.material.color = Color.blue;
                        break;
                    case PowerUpType.INVISIBLE:
                        renderer.material.color = Color.black;
                        break;
                    case PowerUpType.DAMAGE:
                        renderer.material.color = Color.green;
                        break;
                    case PowerUpType.BUILDER:
                        //renderer.material.color = Color.yellow;
                        break;
                }
            }
        }
    }


    #region COMMANDS
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

    [Command]
    void CmdDead()
    {
        IsFrozen = true;
    }

    #endregion

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
        if (isLocalPlayer) {
            switch (other.tag)
            {
                default:
                case StringConstants.pickableTag:
                    CubitsNum++;
                    break;
                case StringConstants.powerupTag:
                    if (powerupIsActive)
                        powerupActivationTime = new DateTime();
                    var rand = new System.Random();
                    
                    switch (rand.Next(0, 2))
                    {
                        default:
                        case 0:
                            if (currentPowerup == PowerUpType.SPEED)
                                goto case 1;
                            currentPowerup = PowerUpType.SPEED;
                            
                            break;
                        case 1:
                            if (currentPowerup == PowerUpType.INVISIBLE)
                                goto case 2;
                            currentPowerup = PowerUpType.INVISIBLE;
                            break;
                        case 2:
                            if (currentPowerup == PowerUpType.DAMAGE)
                                goto case 0;
                            currentPowerup = PowerUpType.DAMAGE;
                            break;
                        //case 3:
                        //    if (currentPowerup == PowerUpType.BUILDER)
                        //        goto case 0;
                        //    currentPowerup = PowerUpType.BUILDER;
                        //    break;
                    }
                    break;
                case StringConstants.sphereTag:
                    myHealthComponent.TakeDamage(25);
                    break;
            }
        }
    }


    public void Dead()
    {
        Debug.Log("Updating Player froze variable");
        CmdDead();
        //freeze
    }

    public void Undead()
    {
        IsFrozen = false;
        //unfreeze
    }


    #region HOOKS
    public void OnMyName(string newName)
    {
        playerName = newName;
        playerNameText.text = playerName;
    }

    public void OnCharacterSelect(int charIndex)
    {
        playerAppearance.SetApperance(charIndex);
    }

    public void OnFrozen(bool toggle)
    {
        IsFrozen = toggle;
    }
    #endregion
}

public enum PowerUpType { NONE, SPEED, DAMAGE, INVISIBLE, BUILDER}