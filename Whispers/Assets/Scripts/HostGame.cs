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
        Debug.Log("Creating room with tag: " + roomCode + " for " + roomSize + " players.");
        // Create room

        networkManager.matchMaker.CreateMatch(roomCode.ToString(), roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
        print("room called " + roomCode + " created");
    }

    public void JoinRoom(MatchInfoSnapshot _match) {
        print("Joining..." + _match.name);
        networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
    }


}
