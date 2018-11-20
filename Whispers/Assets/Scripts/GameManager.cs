using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    //static GameManager _instance;
    //public static GameManager instance{
    //    get{
    //        if(!_instance){
    //            _instance = FindObjectOfType<GameManager>();
    //        }
    //        return _instance;
    //    }
    //}

    public bool nameSet = false;
    public int playersReady;
    //bool started = false;
    //bool drawingNotGuessing = true;

    public float timeToDraw = 60f;
    public float timeToWrite = 30f;

    public int roundNumbr = 0;

    //float startTime = 1;
    public float roundTimer;
    public Slider timerFill;

    RoundDataManager rdm;
    PlayerManager pm;
    //DrawingMachine dm;
    WordGenerator wg;
    //InputManager im;
    GameManager gm;
    UIManager um;
    HostGame hg;

    private void Awake() {
        //rdm = RoundDataManager.instance;
        //pm = PlayerManager.instance;
        //dm = DrawingMachine.instance;
        //wg = WordGenerator.instance;
        //um = UIManager.instance;
        //hg = HostGame.instance;
        //im = InputManager.instance;
        roundTimer = timeToDraw;
        timerFill.maxValue = timeToDraw; 
        //Fabric.EventManager.Instance.PostEvent("tune");
    }
    private void Start() {

    }

    void Update () {
        // if isServer: check if all players are ready and set roundnumbr++ and allPlayers Ready bool to true
    }

    public void PlayerNotReady(){
        var pm = FindObjectOfType<PlayerManager>();
        pm.playerData.playerRDY = false;
    }

    public void Ads() {
        //Advertisement.Show();
    }

    public void GenerateNewWordsToDraw(){ // Sanageneraattorikutsu
        var pco = FindObjectOfType<PlayerConnectionObject>();
        var pm = FindObjectOfType<PlayerManager>();
        var um = FindObjectOfType<UIManager>();
        var wg = FindObjectOfType<WordGenerator>();
        pm.playMode = PlayerManager.PlayMode.Draw;
        um.SetUI();
        PlayerNotReady();
        wg.WordG();
        print("Lisätään ensimmäinen teksti: " + wg.myWord + " ketjuun: " + pm.playerData.playerID);
        pco.CmdAddGuessToChain(wg.myWord, pm.playerData.playerID);
        //roundNumbr++;
        print("RoundNumber ekat: " + roundNumbr);
        um.PocketReset();
        pco.CmdThisClientIsReady();
    }

    public void Gameplay(){
        var um = FindObjectOfType<UIManager>();
        var pm = FindObjectOfType<PlayerManager>();
        print("RoundNumber: " + roundNumbr);
        if(roundNumbr < 10) {
            // See if roundnumber is odd or even and then draw or guess
            if(roundNumbr % 2 == 0) { // if is even, 
                //um.PocketReset();
                pm.playMode = PlayerManager.PlayMode.Write;
                um.SetUI();
                um.ShowPictureToGuess();
                um.ChangeUIText("What on earth is this?");
                SetTimer(timeToWrite);
                PlayerNotReady();
            } else { // odd
                //um.EraseDrawnLines();
                pm.playMode = PlayerManager.PlayMode.Draw;
                um.SetUI();
                um.ShowTextToDraw();
                SetTimer(timeToDraw);
                PlayerNotReady();
            }
        } else {
            // Show chains
            pm.playMode = PlayerManager.PlayMode.Watch;
            um.SetUI();
        }
    }

    void SetTimer(float time){ // Asetetaan ajastin sekä ajastimen koko
        roundTimer = time;
    }


    //void DrawingOrGuessing(){ // Peruspelin vaihtelu
    //    drawingNotGuessing = !drawingNotGuessing;

    //    if(drawingNotGuessing) {
    //        um.EraseDrawnLines();
    //        um.ShowTextToDraw();
    //        pm.playMode = PlayerManager.PlayMode.Draw;
    //        um.SetUI();
    //        SetTimer(timeToDraw);
    //    } else {
    //        um.PocketReset();
    //        um.ShowPictureToGuess(pm.playerData.playerID);
    //        pm.playMode = PlayerManager.PlayMode.Write;
    //        um.SetUI();
    //        um.ChangeUIText("What on earth is this?");
    //        SetTimer(timeToWrite);
    //    }
    //    roundNumbr++;
    //}

    //startTime -= Time.deltaTime; // Introaika
    //if(startTime < 0){ 

    //    if(pm.playerData.playerName == "") { // Onko nimi asetettu
    //        pm.playMode = PlayerManager.PlayMode.Write;
    //        um.SetUI();
    //        um.ChangeUIText("Can you please tell me your name?"); // Asetetaan nimi SendGuess() funktiossa
    //    } else if(!started){
    //        GenerateNewWordsToDraw(); // Ensimmäisen sanasetin luonti
    //        pm.playMode = PlayerManager.PlayMode.Draw;
    //        um.textBox.text = "";
    //        um.SetUI();
    //        nameSet = true;
    //        started = true;
    //    } else{
    //        timerTime -= Time.deltaTime; // Peruspelin looppiajastin
    //    }
    //    if(timerTime <= 0) {
    //        DrawingOrGuessing(); // Peruspeli (Piirretään tai arvataan)
    //    }
    //    timerFill.value = timerTime; // Tiimalasin ajan kuluminen
    //}
}
