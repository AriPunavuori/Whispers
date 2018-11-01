using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HostGame : MonoBehaviour {

    [SerializeField]
    private uint roomSize = 6;
    public string roomName;

    public InputField roomNameInput;    

    NetworkManager networkManager;

    private void Start() {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null) {
            networkManager.StartMatchMaker();
        }
    }

    public void SetRoomName(string name) {
        roomName = name;
    }

    public void CreateRoom() {
        print("trying to create a room");
        if (roomName != "" || roomName == null) {
            Debug.Log("Creating Room: " + roomName + " with room for " + roomSize + " players.");
            // Create room
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
            print("room called " + roomName + " created");
        }
    }


}
