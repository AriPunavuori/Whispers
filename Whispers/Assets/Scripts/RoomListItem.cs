﻿using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour {

    public delegate void JoinRoomDelegate(MatchInfoSnapshot _match);
    JoinRoomDelegate joinRoomCallback;

    MatchInfoSnapshot match;
    [SerializeField]
    Text roomNameText;

    public void Setup(MatchInfoSnapshot _match, JoinRoomDelegate _joinRoomCallback) {
        match = _match;
        joinRoomCallback = _joinRoomCallback;
        roomNameText.text = match.name + " (" + match.currentSize + "/" + match.maxSize + ")";
    }

    public void JoinRoom() {
        joinRoomCallback.Invoke(match);
    }
}
