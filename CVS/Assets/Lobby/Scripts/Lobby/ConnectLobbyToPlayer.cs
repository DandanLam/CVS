using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Prototype.NetworkLobby { 
    public class ConnectLobbyToPlayer : LobbyHook
    {
        public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
        {
            var cc = lobbyPlayer.GetComponent<LobbyPlayer>();
            string name = cc.playerName;
            var player = gamePlayer.GetComponent<Player>();
            player.playerName = name;
            player.characterSel = cc.characterSel;
        }
    }

}