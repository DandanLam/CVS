using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerHUD : NetworkBehaviour {
    
    [SerializeField]
    public Text myCubitsNumberText;
    [SerializeField]
    public Text numPlayerText;
    [SerializeField]
    public Text numSpheresText;

    [SyncVar(hook = "OnChangeNumPlayers")]
    public int numberPlayers;

    [SyncVar(hook = "OnChangeNumSpheres")]
    public int numberSpheres;

    float updateSecond = 1.0f;
    float counter = 0;
    // Update is called once per frame
    void FixedUpdate ()
    {
        if (isLocalPlayer)
        {
            counter += Time.deltaTime;
            if(counter >= updateSecond)
            {
                counter = 0;
                OnChangeNumSpheres(CheckSphereCount());
                OnChangeNumPlayers(CheckPlayerCount());
            }
        }
    }

    int CheckPlayerCount()
    {
        return GameObject.FindGameObjectsWithTag(StringConstants.playerTag).Length;
    }
    int CheckSphereCount()
    {
        return GameObject.FindGameObjectsWithTag(StringConstants.sphereTag).Length;
    }
    void OnChangeNumSpheres(int numOfSpheres)
    {
        numSpheresText.text = numOfSpheres.ToString();
    }
    void OnChangeNumPlayers(int numOfPlayers)
    {
        numPlayerText.text = numOfPlayers.ToString();
    }
}
