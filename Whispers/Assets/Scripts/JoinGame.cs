using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : NetworkBehaviour {

    private NetworkManager networkManager;

    HostGame hg;
    UIManager um;
    public InputField textBox;
    MatchInfoSnapshot foundGame;
    public Text statusText;
    private void Awake() {
        //um = UIManager.instance;
        //hg = HostGame.instance;
    }

    private void Start() {

        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null) {
            networkManager.StartMatchMaker();
        }
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList) {
        if(!success) {
            statusText.text = "Room search unsuccesful.";
            return;
        }
        if (matchList == null) {
            statusText.text = "There is no list.";
            return;
        }
        if(matchList.Count == 0){
            statusText.text = "No rooms found.";
            return;
        }
        print(matchList.Count);
        foundGame = matchList[0];
        JoinRoom(foundGame);
    }

    public void JoinButton() {
        networkManager.matchMaker.ListMatches(0, 20, textBox.text, true, 0, 0, OnMatchList);
    }

    public void JoinRoom(MatchInfoSnapshot _match) {
        statusText.text = "Match found! \nJoining...";
        networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
    }
}
