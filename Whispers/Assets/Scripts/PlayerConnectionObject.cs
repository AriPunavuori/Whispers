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

    public GameObject playerUIPrefab;
    public GameObject UIContainer;

    NetworkIdentity target;

    private void Awake() {
        //um = UIManager.instance;
        //hg = HostGame.instance;
        //pm = PlayerManager.instance;
        //dm = DrawingMachine.instance;
        //gm = GameManager.instance;
        //rdm = RoundDataManager.instance;
    }

    void Start () {
        target = GetComponent<NetworkIdentity>();
        // Is this actually my own local PlayerConnectionObject?
        if (isLocalPlayer == false) {
            // this object belong to another player.
            return; 
        }
        if (isServer) {
            var um = FindObjectOfType<UIManager>();
            var hg = FindObjectOfType<HostGame>();
            um.uiText.text = "Room #" + hg.roomCode;
        }
        CmdAddPlayer();
        CmdShowRoomCode();

        //var PlayerInfo = Instantiate(playerUIPrefab);
        //UIContainer = GameObject.Find("PlayerInfoContainer");
        //PlayerInfo.transform.SetParent(UIContainer.transform);
        //PlayerInfo.transform.localScale = Vector3.one;
    }

    void Update () {
        // update runs on EVERYONE's computer wheter or not they own this particular object
        if (isLocalPlayer == false) {
            return;
        }
    }

    [TargetRpc]
    void TargetSetNetworkId(NetworkConnection _target, int id){
        var pm = FindObjectOfType<PlayerManager>();
        pm.playerData.playerID = id;
        CmdChangeName(pm.playerData.playerID);
    }

    [Command]
    void CmdAddPlayer(){
        var hg = FindObjectOfType<HostGame>();
        hg.numberOfPlayers++;
        RpcUpdatePlayerCount(hg.numberOfPlayers);
        TargetSetNetworkId(target.connectionToClient, hg.numberOfPlayers - 1);
    }

    [ClientRpc]
    void RpcUpdatePlayerCount(int newPlayerCount){
        var hg = FindObjectOfType<HostGame>();
        hg.numberOfPlayers = newPlayerCount;

        var PlayerInfo = Instantiate(playerUIPrefab);
        UIContainer = GameObject.Find("PlayerInfoContainer");
        PlayerInfo.transform.SetParent(UIContainer.transform);
        PlayerInfo.transform.localScale = Vector3.one;
    }

    [Command]
    void CmdShowRoomCode() {
        var hg = FindObjectOfType<HostGame>();
        RpcUpdateRoomCode(hg.roomCode);
        TargetSetNetworkId(target.connectionToClient, hg.numberOfPlayers - 1);
    }
    [ClientRpc]
    void RpcUpdateRoomCode(int roomCode) {
        var um = FindObjectOfType<UIManager>();
        um.uiText.text = "Room# is: " + roomCode;

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
    public void CmdAddGuessToChain(string text, int chainID) {
        rdm = FindObjectOfType<RoundDataManager>();
        rdm.chains[chainID].guesses.Add(text.RemoveDiacritics());
        //print(guesses[gm.roundNumbr/2]);
    }

    [Command]
    public void CmdAddPictureToChain(LineData[] picture, int chainID) {
        rdm = FindObjectOfType<RoundDataManager>();
        rdm.chains[chainID].pictures.Add(picture);
    }

    [Command]
    public void CmdThisClientIsReady() {
        var gm = FindObjectOfType<GameManager>();
        var hg = FindObjectOfType<HostGame>();
        gm.playersReady++;
        if (gm.playersReady >= hg.numberOfPlayers) {
            RpcStartNextRound();
            gm.playersReady = 0;
        }
    }

    [ClientRpc]
    public void RpcStartNextRound() {
        var gm = FindObjectOfType<GameManager>();
        gm.roundNumbr++;
        gm.Gameplay();
    }


    //[Command]
    //void CmdUpdateStringChainDataOnServer(/*WhateverChainData*/){
    //    RpcUpdateStringChaindataOnClients(/*WhateverChainData*/);
    //}

    //[ClientRpc]
    //void RpcUpdateStringChaindataOnClients(/*WhateverChainData*/) {
    //    rdm.AddGuessToChain(rdm.guess, pm.playerData.playerID);
    //}
    //[Command]

    //void CmdUpdatePicChainDataOnServer(/*WhateverChainData*/) {
    //    RpcUpdatePicChaindataOnClients(/*WhateverChainData*/);
    //}

    //[ClientRpc]
    //void RpcUpdatePicChaindataOnClients(/*WhateverChainData*/) {
    //    rdm.AddPictureToChain(dm.lines, pm.playerData.playerID);
    //}
}