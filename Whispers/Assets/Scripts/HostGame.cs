using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

public class HostGame : NetworkBehaviour {


    //static HostGame _instance;
    //public static HostGame instance {
    //    get {
    //        if(!_instance) {
    //            _instance = FindObjectOfType<HostGame>();
    //        }
    //        return _instance;
    //    }
    //}

    [SerializeField]
    uint roomSize = 10;
    public int roomCode;
    public int numberOfPlayers;

    NetworkManager networkManager;
    public GameObject textCont;
    public Text statusText;
    UIManager um;
    PlayerManager pm;


    private void Awake() {
        //um = UIManager.instance;
        //pm = PlayerManager.instance;
    }
    private void Start() {
        //networkManager = NetworkManager.singleton;
        //if (networkManager.matchMaker == null) {
        //    networkManager.StartMatchMaker();
        //}
    }

    public void GenerateRoomCode() {
        roomCode = Random.Range(1000, 9999);
    }

    public void CreateRoom() {
        GenerateRoomCode();
        textCont.SetActive(true);
        textCont.gameObject.GetComponentInChildren<SVGImage>().color = Color.white;
        statusText.text = "Creating room!";
        var networkManager = FindObjectOfType<NetworkManager>();
        if (networkManager.matchMaker == null) {
            networkManager.StartMatchMaker();
        }

        networkManager.matchMaker.CreateMatch(roomCode.ToString(), roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
        var pm = FindObjectOfType<PlayerManager>();
        pm.playerData.playerIsHost = true;
        Fabric.EventManager.Instance.PostEvent("stopmenu");
        Fabric.EventManager.Instance.PostEvent("whisptheme");

    }
}
