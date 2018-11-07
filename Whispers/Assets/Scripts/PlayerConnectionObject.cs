using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectionObject : NetworkBehaviour {
    UIManager um;
    HostGame hg;
    PlayerManager pm;

    private void Awake() {
        um = UIManager.instance;
        hg = HostGame.instance;
        pm = PlayerManager.instance;
    }

    void Start () {
        // Is this actually my own local PlayerConnectionObject?
        if (isLocalPlayer == false) {
            // this object belong to another player.
            return; 
        }
        if(pm.playerData.playerIsHost)
            um.uiText.text = "Room #" + hg.roomCode;
        else
            um.uiText.text = "Wait a second";
    }

	void Update () {
        // update runs on EVERYONE's computer wheter or not they own this particular object
        if (isLocalPlayer == false) {
            return;
        }
    }

    [Command]
    void CmdUpdateChainDataOnServer(){

        RpcUpdateChaindataOnClients();
    }

    [ClientRpc]
    void RpcUpdateChaindataOnClients(){

    }

}
