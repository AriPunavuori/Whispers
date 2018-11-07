using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasButtoneer : MonoBehaviour {

    public GameObject startButton;
    PlayerManager pm;

    private void Awake() {
        pm = PlayerManager.instance;
    }

    private void Start() {
        if(pm.playerData.playerIsHost){
            startButton.gameObject.SetActive(true);
        } else{
            startButton.gameObject.SetActive(false);
        }
    }

}
