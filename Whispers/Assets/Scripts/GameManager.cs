﻿using System.Collections;
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



    public bool nameSet = false;
    bool started = false;
    bool drawingNotGuessing = true;

    public int playerCount = 1;
    public int playerID = 0;

    public float timeToDraw = 60f;
    public float timeToWrite = 30f;

    public int roundNumbr = 1;

    float startTime = 1;
    public float roundTimer;
    public Slider timerFill;

    RoundDataManager rdm;
    PlayerManager pm;
    DrawingMachine dm;
    WordGenerator wg;
    //InputManager im;
    GameManager gm;
    UIManager um;
    HostGame hg;

    private void Awake() {
        rdm = RoundDataManager.instance;
        pm = PlayerManager.instance;
        dm = DrawingMachine.instance;
        wg = WordGenerator.instance;
        um = UIManager.instance;
        hg = HostGame.instance;
        //im = InputManager.instance;
        roundTimer = timeToDraw;
        timerFill.maxValue = timeToDraw; 
        Fabric.EventManager.Instance.PostEvent("tune");
    }
    private void Start() {

    }
    void Update () {





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

    public void Ads() {
        //Advertisement.Show();
    }

    void GenerateNewWordsToDraw(){ // Sanageneraattorikutsu
        wg.WordG();
        rdm.AddGuessToChain(wg.myWord, 0);
        um.PocketReset();
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
}
