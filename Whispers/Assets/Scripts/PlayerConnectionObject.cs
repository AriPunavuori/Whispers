using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Picture = System.Collections.Generic.List<LineData>;

public class PlayerConnectionObject : NetworkBehaviour {
    UIManager um;
    HostGame hg;
    PlayerManager pm;
    RoundDataManager rdm;
    DrawingMachine dm;
    GameManager gm;
    public int playerID;

    private void Awake() {
        um = UIManager.instance;
        hg = HostGame.instance;
        pm = PlayerManager.instance;
        dm = DrawingMachine.instance;
        gm = GameManager.instance;
        rdm = RoundDataManager.instance;
    }

    void Start () {
        // Is this actually my own local PlayerConnectionObject?
        if (isLocalPlayer == false) {
            // this object belong to another player.
            return; 
        }
        if(pm.playerData.playerIsHost){
            //um.uiText.text = "Room #" + hg.roomCode;
        } else
            um.uiText.text = "Wait a second";

        SetNetworkId();
        CmdChangeName(pm.playerData.playerID);
    }

	void Update () {
        // update runs on EVERYONE's computer wheter or not they own this particular object
        if (isLocalPlayer == false) {
            return;
        }
    }

    void SetNetworkId(){
        if (!isServer){
            hg.numberOfPlayers++;
        }
        pm.playerData.playerID = hg.numberOfPlayers;
    }

    [Command]
    void CmdChangeName(int PlayerNumber){
        RpcChangeName(PlayerNumber);
    }

    [ClientRpc]
    void RpcChangeName(int PlayerNumber) {
        transform.name = "Player# " + PlayerNumber;
    }



    [Command]
    void CmdUpdateStringChainDataOnServer(/*WhateverChainData*/){
        RpcUpdateStringChaindataOnClients(/*WhateverChainData*/);
    }

    [ClientRpc]
    void RpcUpdateStringChaindataOnClients(/*WhateverChainData*/) {
        rdm.AddGuessToChain(rdm.guess, pm.playerData.playerID);
    }
    [Command]

    void CmdUpdatePicChainDataOnServer(/*WhateverChainData*/) {
        RpcUpdatePicChaindataOnClients(/*WhateverChainData*/);
    }

    [ClientRpc]
    void RpcUpdatePicChaindataOnClients(/*WhateverChainData*/) {
        rdm.AddPictureToChain(dm.lines, pm.playerData.playerID);
    }
}
