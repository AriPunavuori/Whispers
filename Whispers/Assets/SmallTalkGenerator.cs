using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallTalkGenerator : MonoBehaviour {

    static SmallTalkGenerator _instance;
    public static SmallTalkGenerator instance
    {
        get
        {
            if (!_instance) {
                _instance = FindObjectOfType<SmallTalkGenerator>();
            }
            return _instance;
        }
    }

    public float decProb;
    public float askProb;

    public Text SMTalk;

    public TextAsset declare;
    public TextAsset asking;
    public TextAsset question;
    public TextAsset statement;
    
    string[] decList;
    string[] askList;
    string[] quesList;
    string[] stateList;

    string Dec;
    string Ask;
    string Ques;
    string State;
    string Jones;
    public string myFirst;
    public string mySecond;

    PlayerManager pm;
    UIManager um;

    private void Awake() {
        pm = PlayerManager.instance;
        um = UIManager.instance;

    }
    void Start() {
        //decList = declare.text.Split((string[])null, System.StringSplitOptions.RemoveEmptyEntries);
        //askList = asking.text.Split((string[])null, System.StringSplitOptions.RemoveEmptyEntries);
        //quesList = question.text.Split((string[])null, System.StringSplitOptions.RemoveEmptyEntries);
        //stateList = statement.text.Split((string[])null, System.StringSplitOptions.RemoveEmptyEntries);
        decList = declare.text.Split('\n');
        askList = asking.text.Split('\n');
        quesList = question.text.Split('\n');
        stateList = statement.text.Split('\n');
        Jones = pm.playerData.playerName;
    }

    private void Update() {
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

        SMTalk.text = (myFirst + "\n" + Jones + "\n" + mySecond);
    }
}
