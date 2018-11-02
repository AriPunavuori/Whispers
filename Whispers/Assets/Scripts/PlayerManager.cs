﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    static PlayerManager _instance;
    public static PlayerManager instance{
        get{
            if(!_instance){
                _instance = FindObjectOfType<PlayerManager>();
            }
            return _instance;
        }
    }

    [System.Serializable]
	public struct PlayerData { 
        public string playerName;
        public int playerID;
        public List<float> playerIMG;

        public PlayerData(string playerName, int playerID, List<float> playerIMG) {
            this.playerName = playerName;
            this.playerID = playerID;
            this.playerIMG = playerIMG;
        }
    }

    public PlayerData playerData;

    private void Awake() {
        // PlayerPrefs.SetString("Name", ""); // Tällä voi nollata nimen
        if(PlayerPrefs.GetString("Name") == null){
            playerData.playerName = "";
        } else {
            playerData.playerName = PlayerPrefs.GetString("Name");
        }
    }

    public void SetPlayerName(string name){ // Asetetaan nimi
        playerData.playerName = name;
        PlayerPrefs.SetString("Name", name);
    }
}
