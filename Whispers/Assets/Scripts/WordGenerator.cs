using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
 
    public TextAsset adj1;
    public TextAsset adj2;
    public TextAsset noun;

    //public Text wordText;
    //public TextMeshProUGUI wordText;
    string[] adj1List;
    string[] adj2List;
    string[] nounList;

    string Adj1;
    string Adj2;
    string Noun;
    public string myWord;

    public float aProb;
    public float anProb;

    //RoundDataManager rdm;
    //InputManager im;
    //DrawingMachine dm;
    //WordGenerator wg;
    PlayerManager pm;
    //GameManager gm;
    UIManager um;
    public Text SMTalk;
    string Jones;

    private void Awake() {
        pm = PlayerManager.instance;
        //gm = GameManager.instance;
        um = UIManager.instance;
        //dm = DrawingMachine.instance;
        //rdm = RoundDataManager.instance;
        //im = InputManager.instance;

    }
    void Start() {
        adj1List = adj1.text.Split((string[])null, System.StringSplitOptions.RemoveEmptyEntries);
        adj2List = adj2.text.Split((string[])null, System.StringSplitOptions.RemoveEmptyEntries);
        nounList = noun.text.Split((string[])null, System.StringSplitOptions.RemoveEmptyEntries);
        Jones = pm.playerData.playerName;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.W)) {
            WordG();
        }
    }

    public void WordG() { // Sanageneraattori
        aProb = (Random.Range(0f, 1f));
        anProb = (Random.Range(0f, 1f));
        Adj1 = (adj1List[Random.Range(0, adj1List.Length)]);
        Adj2 = (adj2List[Random.Range(0, adj2List.Length)]);
        Noun = (nounList[Random.Range(0, nounList.Length)]);

        if (aProb > anProb) {
            myWord = "an " + Adj1 + " " + Noun;
        } else {
            myWord = "a " + Adj2 + " " + Noun;
        }

        //um.ChangeUIText("Hi " + Jones + ",\n" + "Can you draw\n" + myWord + "?");
        SMTalk.text = ("Hi " + Jones + ",\n" + "Can you draw\n" + myWord + "?");
    }
}
