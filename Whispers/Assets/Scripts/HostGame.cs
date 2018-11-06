using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

public class HostGame : MonoBehaviour {


    static HostGame _instance;
    public static HostGame instance {
        get {
            if(!_instance) {
                _instance = FindObjectOfType<HostGame>();
            }
            return _instance;
        }
    }

    [SerializeField]
    uint roomSize = 6;
    public int roomCode;   

    NetworkManager networkManager;
    public Text statusText;
    UIManager um;


    private void Awake() {
        um = UIManager.instance;
    }
    private void Start() {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null) {
            networkManager.StartMatchMaker();
        }
    }

    public void GenerateRoomCode() {
        roomCode = Random.Range(1000, 9999);
    }

    public void CreateRoom() {
        GenerateRoomCode();
        networkManager.matchMaker.CreateMatch(roomCode.ToString(), roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
        print(roomCode);
        um.uiText.text = "room called " + roomCode + " created";
    }
}
