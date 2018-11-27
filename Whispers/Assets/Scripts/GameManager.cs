using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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
    public int playersReadyToQuit;
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
        pm = FindObjectOfType<PlayerManager>();
    }

    void Update () {
        if(pm.playMode == PlayerManager.PlayMode.Draw || pm.playMode == PlayerManager.PlayMode.Write){
            roundTimer -= Time.deltaTime; // Peruspelin ajastin
            timerFill.value = roundTimer;
            if(roundTimer < 0) {
                var im = FindObjectOfType<InputManager>();
                if(pm.playMode == PlayerManager.PlayMode.Draw) {
                    im.SendDrawing();
                } else {
                    im.SendGuess();
                }
            }
        }
    }

    public void PlayerNotReady(){
        pm.playerData.playerRDY = false;
    }

    public void Ads() {
        //Advertisement.Show();
    }

    public void GenerateNewWordsToDraw(){ // Sanageneraattorikutsu
        var pco = GameObject.Find("" + pm.playerData.playerID).GetComponent<PlayerConnectionObject>();
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
        var hg = FindObjectOfType<HostGame>();
        print("RoundNumber: " + roundNumbr);
        if(roundNumbr < hg.numberOfPlayers + 1) {
            // See if roundnumber is odd or even and then draw or guess
            if(roundNumbr % 2 == 0) { // if is even, 
                //um.PocketReset();
                pm.playMode = PlayerManager.PlayMode.Write;
                um.SetUI();
                timerFill.maxValue = timeToWrite;
                um.ShowPictureToGuess();
                um.ChangeUIText("What on earth is this?");
                SetTimer(timeToWrite);
                PlayerNotReady();
            } else { // odd
                //um.EraseDrawnLines();
                pm.playMode = PlayerManager.PlayMode.Draw;
                um.SetUI();
                timerFill.maxValue = timeToDraw;
                um.ShowTextToDraw();
                SetTimer(timeToDraw);
                PlayerNotReady();
            }
        } else {
            // Show chains
            pm.playMode = PlayerManager.PlayMode.Watch;
            um.SetUI();
            var es = FindObjectOfType<EndScreen>();
            //es.ThreadDisplay();
        }
    }

    void SetTimer(float time){ // Asetetaan ajastin sekä ajastimen koko
        roundTimer = time;
    }

}
