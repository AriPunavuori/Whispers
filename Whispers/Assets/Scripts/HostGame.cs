using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HostGame : MonoBehaviour {

    [SerializeField]
    uint roomSize = 6;
    int roomCode;   

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


}
