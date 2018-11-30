using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Join : MonoBehaviour {

    public GameObject TextCont;
    public Text statusText;

    public void NewJoinGame() {
        var nm = FindObjectOfType<NetworkManager>().GetComponent<JoinGame>();
        nm.JoinButton();
        Fabric.EventManager.Instance.PostEvent("next");
    }
}
