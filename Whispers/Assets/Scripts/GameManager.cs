using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    public Text timerText;
    [SyncVar]
    public float timerTime = 10f;

    public enum PlayMode { Draw, Type, Watch, Menu };
    


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        timerTime -= Time.deltaTime;
        timerText.text = timerTime.ToString();
        if (timerTime <= 0) {
            timerTime = 10f;
        }
	}

    public void Ads() {
        //Advertisement.Show();
    }

}
