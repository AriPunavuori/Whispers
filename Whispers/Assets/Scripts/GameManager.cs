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

    public GameObject pocket;
    public GameObject pocketPrefab;
    public GameObject nameButton;
    public GameObject drawingUI;
    public GameObject writingUI;

    public Slider timerFill;
    public InputField textBox;

    public string guessedText;

    public int roundNumbr = 1;
    public int playerCount = 1;

    bool nameSet = false;
    bool started = false;
    bool drawingNotGuessing = true;

    [SyncVar]
    public float timeToDraw = 60f;
    [SyncVar]
    public float timeToWrite = 30f;

    float timerTime;
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

    private void Start() {

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

    public void SendDrawing(){ // Funktio joka kutsutaan UI-Buttonilla piirto-UI:ssä
        timerTime = 0;
        rdm.AddPictureToChain(dm.lines, 0);
    }

    public void SendGuess() { // Funktio joka kutsutaan UI-Buttonilla kirjoitus-UI:ssä

        if(!nameSet){
            SetName();
            textBox.text = "";
            um.SetUI(true);
        } else{
            guessedText = textBox.text;
            textBox.text = "";
            timerTime = 0;
        }
        rdm.AddGuessToChain(guessedText, pm.playerData.playerID);
    }

    void GenerateNewWordsToDraw(){ // Sanageneraattorikutsu
        wg.WordG();
        rdm.AddGuessToChain(wg.myWord, 0);
        PocketReset();
    }

    public void PocketReset() { // Piirrettyjen viivojen(Peliobjektien) poisto 
        Destroy(pocket);
        pocket = Instantiate(pocketPrefab);
    }

    void ShowPictureToGuess(int playerID){ // Näytetään kuva arvattavaksi
        //dm.ShowDrawnLines();
        var chainIdx = (roundNumbr + playerID - 1) % playerCount;
        print("Number of players: " + playerCount);
        print("Chain: " + chainIdx);
        print("Round: " + roundNumbr);
        var pics = rdm.chains[chainIdx].pictures;
        dm.ShowPicture(pics[roundNumbr/2]);
        roundNumbr++;
    }

    void ShowTextToDraw(){ // Näytetään teksti piirrettäväksi
        um.ChangeUIText("Draw " + guessedText);
        print("Round: " + roundNumbr);
        roundNumbr++;
    }

    void SetTimer(float time){ // Asetetaan ajastin sekä ajastimen koko
        timerTime = time;
        timerFill.maxValue = time;
    }

    public void SetName(){ // Kutsutaan nimenasetusfunktio
        pm.SetPlayerName(textBox.text);
    }


    void DrawingOrGuessing(){ // Peruspelin vaihtelu
        drawingNotGuessing = !drawingNotGuessing;

        if(drawingNotGuessing) {
            dm.EraseDrawnLines();
            ShowTextToDraw();
            um.SetUI(true);
            pm.playMode = PlayerManager.PlayMode.Draw;
            SetTimer(timeToDraw);
        } else {
            PocketReset();
            ShowPictureToGuess(pm.playerData.playerID);
            um.SetUI(false);
            um.ChangeUIText("What on earth is this?");
            pm.playMode = PlayerManager.PlayMode.Write;
            SetTimer(timeToWrite);
        }
    }
}
