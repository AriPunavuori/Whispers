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
    public float firstProb;
    public float secondProb;

    public Text SMTalk;

    public TextAsset aAdjectives;
    public TextAsset anAdjectives;
    public TextAsset artistNouns;
    public TextAsset endAdjectives;
    public TextAsset could;
    public TextAsset snouns;

    string[] aAList;
    string[] anAList;
    string[] artList;
    string[] endAList;
    string[] couldList;
    string[] snounList;

    string aA;
    string anA;
    string art;
    string endA;
    string coul;
    public string a;
    string snoun;
    public string Jones;
    public string aOrAn;
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
        snounList = snouns.text.Split('\n');
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
        firstProb = Random.Range(0f, 1f);
        secondProb = Random.Range(0f, 1f);
        aA = (aAList[Random.Range(0, aAList.Length)]);
        anA = (anAList[Random.Range(0, anAList.Length)]);
        art = (artList[Random.Range(0, artList.Length)]);
        endA = (endAList[Random.Range(0, endAList.Length)]);
        coul = (couldList[Random.Range(0, couldList.Length)]);
        snoun = (snounList[Random.Range(0, snounList.Length)]);

        if (firstProb > secondProb) {
            SMTalk.text = ("This " + snoun + "\n" + "was drawn by " + "\n" + Jones);
        } else {

            if (aProb > anProb) {
                a = "Only a";
                aOrAn = aA;
            } else {
                a = "Only an";
                aOrAn = anA;
            }
            //um.ChangeUIText(a + aOrAn + art + "\n" + Jones + "\n" + coul +  endA);
            SMTalk.text = (a + " " + aOrAn + " " + art + "\n" + Jones + "\n" + coul + " " + endA);
        }
    }
}

