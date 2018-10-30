using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class GameManager : NetworkBehaviour {

    public static GameManager instance;
    public TextMeshProUGUI uiText;
    bool nameSet = false;
    bool started = false;
    bool drawingNotGuessing = true;
    [SyncVar]
    public float timeToDraw = 60f;
    [SyncVar]
    public float timeToWrite = 30f;
    float timerTime;
    float startTime = 1;

    public enum PlayerMode { Draw, Type, Watch, Menu };
    public GameObject pocket;
    public GameObject pocketPrefab;
    public GameObject nameButton;
    public GameObject drawingUI;
    public GameObject writingUI;
    public Slider timerFill;
    public PlayerMode mode;
    public InputField textBox;
    public string guessedText;

    private void Awake() {
        instance = this;
        timerTime = timeToDraw;
        timerFill.maxValue = timeToDraw;
        mode = PlayerMode.Menu;
    }

	void Update () {
        startTime -= Time.deltaTime;
        if(startTime < 0){
            
            if(PlayerManager.instance.playerData.playerName == "") {
                SetUI(false);
                mode = PlayerMode.Type;
                ChangeDrawText("Can you please tell me your name?");
            } else if(!started){
                GenerateNewWordsToDraw();
                mode = PlayerMode.Draw;
                nameSet = true;
                started = true;
            } else{
                timerTime -= Time.deltaTime;
            }
            if(timerTime <= 0) {
                DrawingOrGuessing();
            }
            timerFill.value = timerTime;
        }
    }

    public void Ads() {
        //Advertisement.Show();
    }

    public void SendDrawing(){
        timerTime = 0;
    }

    public void SendGuess(){

        if(!nameSet){
            PlayerManager.instance.SetPlayerName(textBox.text);
            textBox.text = "";
            SetUI(true);
        } else{
            guessedText = textBox.text;
            textBox.text = "";
            timerTime = 0;
        }
    }

    public void ChangeDrawText(string text){
        uiText.text = text;
    }

    void GenerateNewWordsToDraw(){
        WordGenerator.instance.WordG();
        PocketReset();
    }

    public void PocketReset(){
        Destroy(pocket);
        pocket = Instantiate(pocketPrefab);
    }

    void ShowPictureToGuess(){
        DrawingMachine.instance.ShowDrawnLines();
    }

    void ShowTextToDraw(){
        ChangeDrawText("Draw " + guessedText);
    }

    void SetTimer(float time){
        timerTime = time;
        timerFill.maxValue = time;
    }

    public void SetName(){
        PlayerManager.instance.SetPlayerName(textBox.text);
    }

    void SetUI(bool d){
        drawingUI.SetActive(d);
        writingUI.SetActive(!d);
    }


    void DrawingOrGuessing(){
        drawingNotGuessing = !drawingNotGuessing;

        if(drawingNotGuessing) {
            DrawingMachine.instance.EraseDrawnLines();
            ShowTextToDraw();
            SetUI(true);
            mode = PlayerMode.Draw;
            SetTimer(timeToDraw);
        } else {
            PocketReset();
            ShowPictureToGuess();
            SetUI(false);
            ChangeDrawText("What on earth is this?");
            mode = PlayerMode.Type;
            SetTimer(timeToWrite);
        }
    }
}
