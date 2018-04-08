using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
public class UIHandler : MonoBehaviour {
   
    public void QuitGame()
    {
        NetworkManager.singleton.StopHost();
    }
}
