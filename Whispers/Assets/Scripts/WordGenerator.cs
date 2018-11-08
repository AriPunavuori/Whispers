using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WordGenerator : MonoBehaviour {
    static WordGenerator _instance;
    public static WordGenerator instance
    {
        get
        {
            if (!_instance) {
                _instance = FindObjectOfType<WordGenerator>();
            }
            return _instance;
        }
    }

    public float aProb;
    public float anProb;
    public float askProb;
    public float decProb;
    public float firstProb;
    public float secondProb;

    public string a;
    public string aOrAn;
    public string Jones;
    public string myFirst;
    public string mySecond;
    public string myWord;
    public string want;

    string aA;
    string Adj1;
    string Adj2;
    string anA;
    string art;
    string Ask;
    string coul;
    string Dec;
    string endA;
    string Noun;
    string snoun;
    string State;
    string Ques;


    string[] aAList;
    string[] anAList;
    string[] adj1List;
    string[] adj2List;
    string[] artList;
    string[] askList;
    string[] couldList;
    string[] decList;
    string[] endAList;
    string[] nounList;
    string[] quesList;
    string[] separators = new string[] { "\r\n", "\n" };
    string[] snounList;
    string[] stateList;
    string[] wantList;

    public Text SMTalk;

    public TextAsset aAdjectives;
    public TextAsset anAdjectives;
    public TextAsset adj1;
    public TextAsset adj2;
    public TextAsset artistNouns;
    public TextAsset asking;
    public TextAsset could;
    public TextAsset declare;
    public TextAsset endAdjectives;

    public TextAsset noun;
    public TextAsset question;
    public TextAsset snouns;
    public TextAsset statement;
    public TextAsset wants;

    //public Text wordText;
    //public TextMeshProUGUI wordText;

    //InputManager im;
    //DrawingMachine dm;
    //WordGenerator wg;
    //GameManager gm;

    RoundDataManager rdm;
    UIManager um;
    PlayerManager pm;


    private void Awake() {
        pm = PlayerManager.instance;
        rdm = RoundDataManager.instance;
        um = UIManager.instance;
        //gm = GameManager.instance;
        //dm = DrawingMachine.instance;
        //rdm = RoundDataManager.instance;
        //im = InputManager.instance;

    }
    void Start() {
        wantList = wants.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        adj1List = adj1.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        adj2List = adj2.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        nounList = noun.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        aAList = aAdjectives.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        anAList = anAdjectives.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        artList = artistNouns.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        endAList = endAdjectives.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        couldList = could.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        snounList = snouns.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        decList = declare.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        askList = asking.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        quesList = question.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        stateList = statement.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        Jones = pm.playerData.playerName;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.W)) {
            WordG();
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            GuessG();
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            DrawG();
        }

        if (Input.GetKeyDown(KeyCode.G)) {
            TalkG();
        }

    }

    public void TalkG() { // Sanageneraattori
        decProb = Random.Range(0f, 1f);
        askProb = Random.Range(0f, 1f);
        Dec = (decList[Random.Range(0, decList.Length)]);
        Ask = (askList[Random.Range(0, askList.Length)]);
        Ques = (quesList[Random.Range(0, quesList.Length)]);
        State = (stateList[Random.Range(0, stateList.Length)]);
        if (decProb > askProb) {
            myFirst = Dec;
        } else myFirst = Ask;

        if (myFirst == Dec) {
            mySecond = State;
        } else mySecond = Ques;

        //um.ChangeUIText(myFirst + "\n" + Jones + "\n" + mySecond);
        SMTalk.text = (myFirst + "\n" + Jones + "\n" + mySecond);
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

    public void GuessG() { // Sanageneraattori
        want = (wantList[Random.Range(0, wantList.Length)]);

        //um.ChangeUIText(Jones + " " + want + " you to draw " + rdm.guess.RemoveDiacritics());
        SMTalk.text = (Jones + " " + want + " you to draw " + rdm.guess.RemoveDiacritics());
    }

    public void DrawG() { // Sanageneraattori
        aProb = Random.Range(0f, 1f);
        anProb = Random.Range(0f, 1f);
        firstProb = Random.Range(0f, 1f);
        secondProb = Random.Range(0f, 1f);
        aA = (aAList[Random.Range(0, aAList.Length)]);
        anA = (anAList[Random.Range(0, anAList.Length)]);
        art = (artList[Random.Range(0, artList.Length)]);
        endA = (endAList[Random.Range(0, endAList.Length)]);
        coul = (couldList[Random.Range(0, couldList.Length)]);
        snoun = (snounList[Random.Range(0, snounList.Length)]);
        

        if (firstProb > secondProb) {
            //um.ChangeUIText("This " + snoun + "\n" + "was drawn by " + "\n" + Jones);
            SMTalk.text = ("This " + snoun + "\n" + "was drawn by " + "\n" + Jones);
        } else {

            if (aProb > anProb) {
                a = "Only a";
                aOrAn = aA;
            } else {
                a = "Only an";
                aOrAn = anA;
            }
            //um.ChangeUIText(a + " " + aOrAn + " " + art + " like \n" + Jones + "\n" + coul + " " + endA);
            SMTalk.text = (a + " " + aOrAn + " " + art + " like \n" + Jones + "\n" + coul + " " + endA);
        }
    }
}
