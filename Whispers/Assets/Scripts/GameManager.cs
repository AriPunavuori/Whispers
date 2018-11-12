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



    public bool nameSet = false;
    bool allPlayersReady = true;
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

        if(allPlayersReady){
            if(roundNumbr == 1){
                // Show first words to draw
                pm.playMode = PlayerManager.PlayMode.Draw;
                um.SetUI();
                PlayerNotReady();
                roundNumbr++;
                GenerateNewWordsToDraw(); // Ensimmäisen sanasetin luonti
                //um.textBox.text = "";
            } else if (roundNumbr < hg.numberOfPlayers){
                // See if roundnumber is odd or even and then draw or guess
                if(roundNumbr % 2 == 0){
                    um.PocketReset();
                    um.ShowPictureToGuess(pm.playerData.playerID);
                    pm.playMode = PlayerManager.PlayMode.Write;
                    um.SetUI();
                    um.ChangeUIText("What on earth is this?");
                    SetTimer(timeToWrite);
                    PlayerNotReady();
                    roundNumbr++;
                } else {
                    um.EraseDrawnLines();
                    um.ShowTextToDraw();
                    pm.playMode = PlayerManager.PlayMode.Draw;
                    um.SetUI();
                    SetTimer(timeToDraw);
                    PlayerNotReady();
                    roundNumbr++;
                }
            } else{
                // Show chains
                pm.playMode = PlayerManager.PlayMode.Watch;
                um.SetUI();
            }
            // if isServer: check if all players are ready and set roundnumbr++ and allPlayersReady bool to true
        }



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
    public void PlayerNotReady(){
        allPlayersReady = false;
        pm.playerData.playerRDY = false;
    }

    public void Ads() {
        //Advertisement.Show();
    }

    void GenerateNewWordsToDraw(){ // Sanageneraattorikutsu
        wg.WordG();
        print(wg.myWord);
        print("PlayerIDSlotInChain: " + playerID);
        //rdm.AddGuessToChain(wg.myWord, pm.playerData.playerID);
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
