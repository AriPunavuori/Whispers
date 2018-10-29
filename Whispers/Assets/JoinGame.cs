using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {

    private NetworkManager networkManager;
    List<GameObject> roomList = new List<GameObject>();
    [SerializeField]
    Text statusText;
    [SerializeField]
    GameObject roomListItemPrefab;
    [SerializeField]
    Transform roomListParent;

    private void Start() {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null) {
            networkManager.StartMatchMaker();
        }

        RefreshRoomList();

    }

    public void RefreshRoomList() {
        ClearRoomList();
        networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
        statusText.text = "Loading...";
    }



    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList) {
        statusText.text = "";
        if (!success || matchList == null) {
            statusText.text = "No rooms found.";
            return;
        }

        foreach (MatchInfoSnapshot match in matchList) {
            GameObject _roomListItemGO = Instantiate(roomListItemPrefab);
            _roomListItemGO.transform.SetParent(roomListParent);

            RoomListItem _roomListItem = _roomListItemGO.GetComponent<RoomListItem>();
            if (_roomListItem != null) {
                _roomListItem.Setup(match, JoinRoom);
            }

            roomList.Add(_roomListItemGO);
        }
        if (roomList.Count == 0) {
            statusText.text = "No rooms at the moment.";
        }
    }

    void ClearRoomList() {
        for (int i = 0; i < roomList.Count; i++) {
            Destroy(roomList[i]);
        }
        roomList.Clear();
    }
    
    public void JoinRoom(MatchInfoSnapshot _match) {
        print("Joining..." + _match.name);
        networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        ClearRoomList();
        statusText.text = "JOINING...";
    }
}
