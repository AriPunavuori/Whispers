using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WordGenerator : MonoBehaviour {
    static WordGenerator _instance;
    public static WordGenerator instance{
        get{
            if(!_instance){
                _instance = FindObjectOfType<WordGenerator>();
            }
            return _instance;
        }
    }
 
    public TextAsset adj;
    public TextAsset noun;

    //public Text wordText;
    //public TextMeshProUGUI wordText;
    string[] adjList;
    string[] nounList;

    string Adj;
    string Noun;
    public string myWord;

    RoundDataManager rdm;
    PlayerManager pm;
    DrawingMachine dm;
    WordGenerator wg;
    InputManager im;
    GameManager gm;

    private void Awake() {
        pm = PlayerManager.instance;
        dm = DrawingMachine.instance;
        rdm = RoundDataManager.instance;
        im = InputManager.instance;
        gm = GameManager.instance;
    }
    void Start() {
        adjList = adj.text.Split((string[])null, System.StringSplitOptions.RemoveEmptyEntries);
        nounList = noun.text.Split((string[])null, System.StringSplitOptions.RemoveEmptyEntries);
    }

    public void WordG() { // Sanageneraattori
        Adj = (adjList[Random.Range(0, adjList.Length)]);
        Noun = (nounList[Random.Range(0, nounList.Length)]);
        myWord = Adj + " " + Noun;
        //print(Adj.Length);
        gm.ChangeUIText("Hi " + pm.playerData.playerName + ", Can you draw:\n" + myWord + "?");
    }
}
