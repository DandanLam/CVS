using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

namespace Prototype.NetworkLobby { 
    public class LobbyPasswordInput : MonoBehaviour {

        public Text infoText;
        public Text buttonText;
        public InputField passwordText;
        public Button singleButton;

        public void Display(string info, string buttonInfo, LobbyServerEntry entry, NetworkID networkID, LobbyManager lobbyManager)
        {
            infoText.text = info;
            buttonText.text = buttonInfo;
            singleButton.onClick.RemoveAllListeners();
            singleButton.onClick.AddListener(() => { entry.JoinMatch(networkID, lobbyManager, passwordText.text); });
            singleButton.onClick.AddListener(() => { gameObject.SetActive(false); });

            gameObject.SetActive(true);
        }
    }
}
