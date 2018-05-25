using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System.Collections;

namespace Prototype.NetworkLobby
{
    public class LobbyServerEntry : MonoBehaviour 
    {
        public Text serverInfoText;
        public Text slotInfo;
        public Button joinButton;
        public RectTransform lockIcon;
        public bool isPrivateMatch = false;

		public void Populate(MatchInfoSnapshot match, LobbyManager lobbyManager, Color c)
		{
            isPrivateMatch = match.isPrivate;
            lockIcon.gameObject.SetActive(match.isPrivate);
            serverInfoText.text = match.name;
            Debug.LogError("populate list with network id: " + match.networkId);

            slotInfo.text = match.currentSize.ToString() + "/" + match.maxSize.ToString(); ;

            NetworkID networkID = match.networkId;

            joinButton.onClick.RemoveAllListeners();
            joinButton.onClick.AddListener(() => { JoinWithPassword(networkID, lobbyManager, ""); });

            GetComponent<Image>().color = c;
        }

        public void JoinMatch(NetworkID networkID, LobbyManager lobbyManager, string password)
        {
			lobbyManager.matchMaker.JoinMatch(networkID, password, "", "", 0, 0, lobbyManager.OnMatchJoined);
			lobbyManager.backDelegate = lobbyManager.StopClientClbk;
            lobbyManager._isMatchmaking = true;
            lobbyManager.DisplayIsConnecting();
        }

        void JoinWithPassword(NetworkID networkID, LobbyManager lobbyManager, string password)
        {
            if (isPrivateMatch)
            {
                lobbyManager.DisplayInputPassword(this, networkID, lobbyManager);
            } 
            else
            {
                JoinMatch(networkID, lobbyManager, password);
            }
        }
    }
}