using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessGenerator : MonoBehaviour {

    static GuessGenerator _instance;
    public static GuessGenerator instance
    {
        get
        {
            if (!_instance) {
                _instance = FindObjectOfType<GuessGenerator>();
            }
            return _instance;
        }
    }

    public Text SMTalk;

    public TextAsset wants;

    string[] wantList;

    public string want;

    public string Jones;

    string[] separators = new string[] { "\r\n", "\n" };

    PlayerManager pm;
    UIManager um;
    RoundDataManager rdm;

    private void Awake() {
        pm = PlayerManager.instance;
        um = UIManager.instance;
        rdm = RoundDataManager.instance;
    }
    void Start() {
        wantList = wants.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        Jones = pm.playerData.playerName;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            GuessG();
        }
    }

    public void GuessG() { // Sanageneraattori
        want = (wantList[Random.Range(0, wantList.Length)]);

        //um.ChangeUIText(Jones + " " + want + " you to draw " + rdm.guess);
        SMTalk.text = (Jones + " " + want + " you to draw ");//+rdm.guess);
    }
}

