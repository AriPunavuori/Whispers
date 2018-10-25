using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectionObject : NetworkBehaviour {

    public GameObject PlayerUnitPrefab;
    Canvas canvas;
    //// SyncVar are variables where if their value changes on the SERVER, then all clients are
    //// automatically informed of the new value.
    //[SyncVar (hook ="OnPlayerNameChanged") ]
    public string PlayerName = "Player#";

	void Start () {
        // Is this actually my own local PlayerConnectionObject?
        if (isLocalPlayer == false) {
            // this object belong to another player.
            return; 
        }

        canvas = FindObjectOfType<Canvas>();
        transform.parent = canvas.transform;

        // Instantiate() only creates an object on the LOCAL COMPUTER.
        // Even it has a NetworkIdentity it still will NOT exist on the network (and therefore not on any other client)
        // UNLESS NetworkServer.Spawn() is called on this object.

        // Instantiate(PlayerUnitPrefab);

        // Command the server to SPAWN our unit.
        CmdSpawnMyUnit();
	}

	void Update () {
        // update runs on EVERYONE's computer wheter or not they own this particular object

        if (isLocalPlayer == false) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            CmdSpawnMyUnit();
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            string name = "Nikle" + Random.Range(1, 100);
            print("Sending a server request to change player name to: " + name);
            CmdChangePlayerName(name);
        }
    }

    //With SyncVar Hook code:
    //void OnPlayerNameChange(string newName) {
    //    print("OnPlayerNameChanged: OldName: " + PlayerName + " , NewName: " + newName);

    //    // If SyncVar is used with hook, then our local value does NOT update automatically
    //    // so we do it manually
    //    PlayerName = newName;

    //    gameObject.name = "PlayerConnectionObject [" + newName + "]";
    //}

    /////////////////// COMMANDS
    // special functions that is executed ONLY on the server

    [Command]
    void CmdSpawnMyUnit() {
        // we are guaranteed to be on server right now.

        GameObject go = Instantiate(PlayerUnitPrefab);

        // Now that the object exists on the server, propagate it to all clients (and also wire up the NetworkIdentity).
        //NetworkServer.Spawn(go);
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }
    [Command]
    void CmdChangePlayerName(string name) {
        print("Name changed to:" + name);
        PlayerName = name;

        // Tell all clients what this player's name is now.
        RpcChangePlayerName(PlayerName);
    }

    /////////////////// RPC
    // special functions that is executed ONLY on the clients.

    [ClientRpc]
    void RpcChangePlayerName(string name) {
        print("RpcChangePlayerName: We were asked to change the player name on a particular PlayerConnectionObject: " + name);
        PlayerName = name;
    }
}
