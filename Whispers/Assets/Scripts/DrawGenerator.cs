using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawGenerator : MonoBehaviour {

    static DrawGenerator _instance;
    public static DrawGenerator instance
    {
        get
        {
            if (!_instance) {
                _instance = FindObjectOfType<DrawGenerator>();
            }
            return _instance;
        }
    }

    public float aProb;
    public float anProb;

    public Text SMTalk;

    public TextAsset aAdjectives;
    public TextAsset anAdjectives;
    public TextAsset artistNouns;
    public TextAsset endAdjectives;
    public TextAsset could;

    string[] aAList;
    string[] anAList;
    string[] artList;
    string[] endAList;
    string[] couldList;

    string aA;
    string anA;
    string art;
    string endA;
    string coul;
    string a;
    public string Jones;
    public string aOrAn;
    public string artistAdj;
    public string endAdj;
    public string couldL;
    string[] separators = new string[] { "\r\n", "\n" };

    PlayerManager pm;
    UIManager um;

    private void Awake() {
        pm = PlayerManager.instance;
        um = UIManager.instance;

    }
    void Start() {
        aAList = aAdjectives.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        anAList = anAdjectives.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        artList = artistNouns.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        endAList = endAdjectives.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        couldList = could.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        Jones = pm.playerData.playerName;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.D)) {
            DrawG();
        }
    }

    public void DrawG() { // Sanageneraattori
        aProb = Random.Range(0f, 1f);
        anProb = Random.Range(0f, 1f);
        aA = (aAList[Random.Range(0, aAList.Length)]);
        anA = (anAList[Random.Range(0, anAList.Length)]);
        art = (artList[Random.Range(0, artList.Length)]);
        endA = (endAList[Random.Range(0, endAList.Length)]);
        coul = (couldList[Random.Range(0, couldList.Length)]);

        if (aProb > anProb) {
            a = "Only a";
            aOrAn = aA;
        } else {
            a = "Only an";
            aOrAn = anA;

            if (aOrAn == aA) { }
            //um.ChangeUIText(a + aOrAn + art + "\n" + Jones + "\n" + coul +  endA);
            SMTalk.text = (a + " " + aOrAn + " " + art + "\n" + Jones + "\n" + coul + " " + endA);
        }
    }
}
