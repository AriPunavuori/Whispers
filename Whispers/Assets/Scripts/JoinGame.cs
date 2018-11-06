using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {

    private NetworkManager networkManager;

    HostGame hg;
    UIManager um;
    public InputField textBox;
    MatchInfoSnapshot foundGame;
    public Text statusText;
    private void Awake() {
        um = UIManager.instance;
        hg = HostGame.instance;
    }

    private void Start() {

        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null) {
            networkManager.StartMatchMaker();
        }
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList) {
        if(!success) {
            um.uiText.text = "Room search unsuccesful.";
            return;
        }
        if (matchList == null) {
            um.uiText.text = "There is no list.";
            return;
        }
        if(matchList.Count == 0){
            um.uiText.text = "No rooms found";
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
        um.uiText.text = "Trying to join found match.";
        networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
    }
}
