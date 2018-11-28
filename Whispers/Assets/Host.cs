using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
public class Host : MonoBehaviour {

    public void NewHostGame(){
        var nm = FindObjectOfType<NetworkManager>().GetComponent<HostGame>();
        nm.CreateRoom();
    }
}
