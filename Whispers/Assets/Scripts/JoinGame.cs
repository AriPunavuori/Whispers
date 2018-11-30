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
    public GameObject TextCont;
    public Text statusText;
    private void Awake() {
        //um = UIManager.instance;
        //hg = HostGame.instance;
    }

    private void Start() {
        Fabric.EventManager.Instance.PostEvent("tune");
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null) {
            networkManager.StartMatchMaker();
        }
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList) {
        if(!success) {
            TextCont.SetActive(true);
            Fabric.EventManager.Instance.PostEvent("error2");
            statusText.text = "Room search unsuccesful.";
            return;
        }
        if (matchList == null) {
            Fabric.EventManager.Instance.PostEvent("error2");
            TextCont.SetActive(true);
            statusText.text = "There is no list.";
            return;
        }
        if(matchList.Count == 0){
            Fabric.EventManager.Instance.PostEvent("error2");
            TextCont.SetActive(true);
            statusText.text = "No rooms found.";
            return;
        }
        print("huoneita olemassa nyt" + matchList.Count);
        foundGame = matchList[0];
        JoinRoom(foundGame);
    }

    public void JoinButton() {
        networkManager.matchMaker.ListMatches(0, 20, textBox.text, true, 0, 0, OnMatchList);
    }

    public void JoinRoom(MatchInfoSnapshot _match) {
        TextCont.SetActive(true);
        statusText.text = "Match found! \nJoining...";
        Fabric.EventManager.Instance.PostEvent("stopmenu");
        Fabric.EventManager.Instance.PostEvent("next");
        Fabric.EventManager.Instance.PostEvent("whisptheme");
        networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
    }
}
