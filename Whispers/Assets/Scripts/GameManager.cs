using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    static GameManager _instance;
    public static GameManager instance{
        get{
            if(!_instance){
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    public GameObject nameButton;

    public Slider timerFill;

    public int roundNumbr = 1;
    public int playerCount = 1;

    public bool nameSet = false;
    bool started = false;
    bool drawingNotGuessing = true;

    [SyncVar]
    public float timeToDraw = 60f;
    [SyncVar]
    public float timeToWrite = 30f;

    public float timerTime;
    float startTime = 1;

    RoundDataManager rdm;
    PlayerManager pm;
    DrawingMachine dm;
    WordGenerator wg;
    //InputManager im;
    GameManager gm;
    UIManager um;

    private void Awake() {
        rdm = RoundDataManager.instance;
        pm = PlayerManager.instance;
        dm = DrawingMachine.instance;
        wg = WordGenerator.instance;
        um = UIManager.instance;
        //im = InputManager.instance;
        timerTime = timeToDraw;
        timerFill.maxValue = timeToDraw; 
        Fabric.EventManager.Instance.PostEvent("tune");
    }

    void Update () {
        startTime -= Time.deltaTime; // Introaika
        if(startTime < 0){ 

            if(pm.playerData.playerName == "") { // Onko nimi asetettu
                um.SetUI(false);
                pm.playMode = PlayerManager.PlayMode.Write;
                um.ChangeUIText("Can you please tell me your name?"); // Asetetaan nimi SendGuess() funktiossa
            } else if(!started){
                GenerateNewWordsToDraw(); // Ensimmäisen sanasetin luonti
                pm.playMode = PlayerManager.PlayMode.Draw;
                um.textBox.text = "";
                um.SetUI(true);
                nameSet = true;
                started = true;
            } else{
                timerTime -= Time.deltaTime; // Peruspelin looppiajastin
            }
            if(timerTime <= 0) {
                DrawingOrGuessing(); // Peruspeli (Piirretään tai arvataan)
            }
            timerFill.value = timerTime; // Tiimalasin ajan kuluminen
        }
    }

    public void Ads() {
        //Advertisement.Show();
    }

    void GenerateNewWordsToDraw(){ // Sanageneraattorikutsu
        wg.WordG();
        rdm.AddGuessToChain(wg.myWord, 0);
        um.PocketReset();
    }

    void SetTimer(float time){ // Asetetaan ajastin sekä ajastimen koko
        timerTime = time;
        timerFill.maxValue = time;
    }

    void DrawingOrGuessing(){ // Peruspelin vaihtelu
        drawingNotGuessing = !drawingNotGuessing;

        if(drawingNotGuessing) {
            um.EraseDrawnLines();
            um.ShowTextToDraw();
            um.SetUI(true);
            pm.playMode = PlayerManager.PlayMode.Draw;
            SetTimer(timeToDraw);
        } else {
            um.PocketReset();
            um.ShowPictureToGuess(pm.playerData.playerID);
            um.SetUI(false);
            um.ChangeUIText("What on earth is this?");
            pm.playMode = PlayerManager.PlayMode.Write;
            SetTimer(timeToWrite);
        }
        roundNumbr++;
    }
}
