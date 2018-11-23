using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Demoend : MonoBehaviour {
    static Demoend _instance;
    public static Demoend instance
    {
        get
        {
            if (!_instance) {
                _instance = FindObjectOfType<Demoend>();
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

    public TextMeshProUGUI SMTalk;

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
    DemoStyleEnd dse;


    private void Awake() {
        //pm = PlayerManager.instance;
        //rdm = RoundDataManager.instance;
        //um = UIManager.instance;
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
        var pm = FindObjectOfType<PlayerManager>();
        dse = FindObjectOfType<DemoStyleEnd>();
        Jones = pm.playerData.playerName;
        var ch = rdm.chains[dse.chain];
    }

    //public void EndGuess() {
    //    var rnd = Random.Range(0, 3);
    //    //float a = Random.value;
    //    //float b = Random.value;
    //    //float c = Random.value;
    //    //float d = Random.value;
    //    //var values = new List<float>() { a, b, c, d };
    //    //var max = values.Max();
    //    //var idx = values.IndexOf(max);

    //    if (dse.round == 0) {
    //        SMTalk.text = (Jones3 + " wanted " + Jones2 + " to draw " + rdm.guess.RemoveDiacritics());

    //    } else {
    //        if (rnd == 0) {
    //            SMTalk.text = ("which looked like " + ch.guess[0] + " to " + Jones2);
    //        } else if (rnd == 1) {
    //            SMTalk.text = ("which reminded " + Jones2 + " of " + +ch.guess[0]);
    //        } else if (rnd == 2) {
    //            SMTalk.text = ("which  " + Jones2 + " interpreted as " + ch.guess[0]);
    //        }
    //    }
    //}

    public void EndPic() {
        aProb = Random.Range(0f, 1f);
        anProb = Random.Range(0f, 1f);
        firstProb = Random.Range(0f, 1f);
        secondProb = Random.Range(0f, 1f);

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
            //um.ChangeUIText(a + " " + aOrAn + " " + art + " like \n" + Jones + "\n" + coul + " " + endA);
            SMTalk.text = (a + " " + aOrAn + " " + art + " like \n" + Jones + "\n" + coul + " " + endA);
        }
    }
}
