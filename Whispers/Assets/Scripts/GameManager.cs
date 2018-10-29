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
    // public Text timerText;
    bool started = false;
    bool drawingNotGuessing = true;
    [SyncVar]
    public float timeToDraw = 60f;
    public float timeToWrite = 30f;
    float timerTime;
    float startTime = 1;

    public enum PlayerMode { Draw, Type, Watch, Menu };
    public GameObject pocket;
    public GameObject pocketPrefab;
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
            if(!started){
                GenerateNewWordsToDraw();
                mode = PlayerMode.Draw;
                started = true;
            }
            timerTime -= Time.deltaTime;
            //timerText.text = timerTime.ToString();

            if(timerTime <= 0) {

                drawingNotGuessing =! drawingNotGuessing;

                if(drawingNotGuessing){
                    DrawingMachine.instance.EraseDrawedLines();
                    ShowTextToDraw();
                    drawingUI.SetActive(true);
                    writingUI.SetActive(false);
                    mode = PlayerMode.Draw;
                    SetTimer(timeToDraw);
                } else{
                    PocketReset();
                    ShowPictureToGuess();
                    drawingUI.SetActive(false);
                    writingUI.SetActive(true);
                    ChangeDrawText("What on earth is this?");
                    mode = PlayerMode.Type;
                    SetTimer(timeToWrite);
                }
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
        guessedText = textBox.text;
        textBox.text = "";
        timerTime = 0;
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
        DrawingMachine.instance.ShowDrawedLines();
    }

    void ShowTextToDraw(){
        ChangeDrawText("Draw " + guessedText);
    }

    void SetTimer(float time){
        timerTime = time;
        timerFill.maxValue = time;
    }

}
