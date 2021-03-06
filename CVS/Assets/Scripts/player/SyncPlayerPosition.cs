﻿using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class SyncPlayerPosition : NetworkBehaviour {
    [SyncVar]
    private Vector3 syncPos;

    [SyncVar]
    private Quaternion syncRot;

    [SerializeField]
    Transform myTransform;

    [SerializeField]
    float lerpRate = 15;


    private void FixedUpdate()
    {
        TransmitPosition();
        LerpPosition();
    }

    void LerpPosition()
    {
        if (!isLocalPlayer)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
            myTransform.rotation = Quaternion.Lerp(myTransform.rotation, syncRot, Time.deltaTime * lerpRate);
        }
    }
    
    [Command]
    void CmdProvidePositionToServer(Vector3 pos, Quaternion rot)
    {
        syncPos = pos;
        syncRot = rot;
    }
    
    void TransmitPosition(){
        if (isServer) { 
            CmdProvidePositionToServer(myTransform.position, myTransform.rotation);
        }
    }
}
