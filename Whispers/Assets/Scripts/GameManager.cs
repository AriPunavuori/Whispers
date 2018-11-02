using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

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

    public TextMeshProUGUI uiText;

    public enum PlayerMode { Draw, Type, Watch, Menu }; // Onko tarpeellinen?

    public GameObject pocket;
    public GameObject pocketPrefab;
    public GameObject nameButton;
    public GameObject drawingUI;
    public GameObject writingUI;

    public Slider timerFill;
    public PlayerMode mode;
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

    private void Awake() {
        timerTime = timeToDraw;
        timerFill.maxValue = timeToDraw; 
        mode = PlayerMode.Menu;
        Fabric.EventManager.Instance.PostEvent("tune");
    }

	void Update () {
        startTime -= Time.deltaTime; // Introaika
        if(startTime < 0){ 
            
            if(PlayerManager.instance.playerData.playerName == "") { // Onko nimi asetettu
                SetUI(false);
                mode = PlayerMode.Type;
                ChangeUIText("Can you please tell me your name?"); // Asetetaan nimi SendGuess() funktiossa
            } else if(!started){
                GenerateNewWordsToDraw(); // Ensimmäisen sanasetin luonti

                mode = PlayerMode.Draw;
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
        RoundDataManager.instance.AddPicture(DrawingMachine.instance.lines, 0);
    }

    public void SendGuess() { // Funktio joka kutsutaan UI-Buttonilla kirjoitus-UI:ssä

        if(!nameSet){
            SetName();
            textBox.text = "";
            SetUI(true);
        } else{
            guessedText = textBox.text;
            textBox.text = "";
            timerTime = 0;
        }
        RoundDataManager.instance.AddGuess(guessedText, PlayerManager.instance.playerData.playerID);
    }

    public void ChangeUIText(string text){ // UI-Tekstin vaihto
        uiText.text = text;
    }

    void GenerateNewWordsToDraw(){ // Sanageneraattorikutsu
        WordGenerator.instance.WordG();
        RoundDataManager.instance.AddGuess(WordGenerator.instance.myWord, 0);
        PocketReset();
    }

    public void PocketReset() { // Piirrettyjen viivojen(Peliobjektien) poisto 
        Destroy(pocket);
        pocket = Instantiate(pocketPrefab);
    }

    void ShowPictureToGuess(int playerID){ // Näytetään kuva arvattavaksi
        //DrawingMachine.instance.ShowDrawnLines();
        var chainIdx = (roundNumbr + playerID - 1) % playerCount;
        var rdm = RoundDataManager.instance;
        print("Number of players: " + playerCount);
        print("Chain: " + chainIdx);
        print("Round: " + roundNumbr);
        var pics = rdm.chains[chainIdx].pictures;
        rdm.ShowPicture(pics[roundNumbr/2]);
        roundNumbr++;
    }

    void ShowTextToDraw(){ // Näytetään teksti piirrettäväksi
        ChangeUIText("Draw " + guessedText);
        print("Round: " + roundNumbr);
        roundNumbr++;
    }

    void SetTimer(float time){ // Asetetaan ajastin sekä ajastimen koko
        timerTime = time;
        timerFill.maxValue = time;
    }

    public void SetName(){ // Kutsutaan nimenasetusfunktio
        PlayerManager.instance.SetPlayerName(textBox.text);
    }

    void SetUI(bool d){ // Vaihdetaan UI-Näkymää
        drawingUI.SetActive(d);
        writingUI.SetActive(!d);
    }

    void DrawingOrGuessing(){ // Peruspelin vaihtelu
        drawingNotGuessing = !drawingNotGuessing;

        if(drawingNotGuessing) {
            DrawingMachine.instance.EraseDrawnLines();
            ShowTextToDraw();
            SetUI(true);
            mode = PlayerMode.Draw;
            SetTimer(timeToDraw);
        } else {
            PocketReset();
            ShowPictureToGuess(PlayerManager.instance.playerData.playerID);
            SetUI(false);
            ChangeUIText("What on earth is this?");
            mode = PlayerMode.Type;
            SetTimer(timeToWrite);
        }
    }
}
