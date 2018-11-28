using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
public class Join : MonoBehaviour {

    public void NewJoinGame() {
        var nm = FindObjectOfType<NetworkManager>().GetComponent<JoinGame>();
        nm.JoinButton();
        Fabric.EventManager.Instance.PostEvent("button1");
    }
}
