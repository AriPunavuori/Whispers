using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CanvasButtoneer : NetworkBehaviour {

    public GameObject startButton;
    PlayerManager pm;
    UIManager um;

    private void Awake() {
        pm = PlayerManager.instance;
        um = UIManager.instance;
    }

    private void Start() {
        //if(pm.playerData.playerIsHost){
        //    startButton.gameObject.SetActive(true);
        //} else{
        //    startButton.gameObject.SetActive(false);
        //}
    }


    [Command]
    void CmdUpdateChainDataOnServer() {
        RpcUpdateChaindataOnClients();
    }

    [ClientRpc]
    void RpcUpdateChaindataOnClients() {
        um.uiText.text = "Holla!";
    }
}
