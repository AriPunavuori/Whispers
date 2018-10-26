using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class GameManager : NetworkBehaviour {

    public static GameManager instance;
    public TextMeshProUGUI drawText;
    public Text timerText;
    [SyncVar]
    public float timerTime = 10f;
    public enum PlayMode { Draw, Type, Watch, Menu };

    private void Awake() {
        instance = this;
    }

	void Update () {

        timerTime -= Time.deltaTime;
        //timerText.text = timerTime.ToString();
        if (timerTime <= 0) {
            timerTime = 10f;
        }
	}

    public void Ads() {
        //Advertisement.Show();
    }

    public void ChangeDrawText(string text){
        drawText.text = text;
    }

}
