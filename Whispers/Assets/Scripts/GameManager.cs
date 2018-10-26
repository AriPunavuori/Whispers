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
    bool started = false;
    [SyncVar]
    float timeToDraw = 10f;
    public float timerTime;
    float startTime = 2;
    public enum PlayMode { Draw, Type, Watch, Menu };
    public GameObject pocket;
    public GameObject pocketPrefab;
    public Slider timerFill;


    private void Awake() {
        instance = this;
        timerTime = timeToDraw;
        timerFill.maxValue = timeToDraw;
    }

	void Update () {
        startTime -= Time.deltaTime;
        if(startTime<0){
            if(!started){
                GenerateNewWordsToDraw();
                started = true;
            }
            timerTime -= Time.deltaTime;
            //timerText.text = timerTime.ToString();

            if(timerTime <= 0) {
                GenerateNewWordsToDraw();
            }
            timerFill.value = timerTime;
        }

    }

    public void Ads() {
        //Advertisement.Show();
    }

    public void ChangeDrawText(string text){
        drawText.text = text;
    }

    void GenerateNewWordsToDraw(){
        Destroy(pocket);
        pocket = Instantiate(pocketPrefab);
        timerTime = timeToDraw;
        WordGenerator.instance.WordG();
    }



}
