using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayerInfo : MonoBehaviour {

    PlayerManager pm;

    public Text nameText;
    Image playerIcon;

    private void Awake() {
        //pm = PlayerManager.instance;
        playerIcon = GetComponentInChildren<Image>();

    }

    void Start () {
        GetPlayerName();
        RandomizeIconColor();
    }
	

    void GetPlayerName() {
        var pm = FindObjectOfType<PlayerManager>();
        nameText.text = pm.playerData.playerName; 
    }

    void RandomizeIconColor() {
        playerIcon.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

}
