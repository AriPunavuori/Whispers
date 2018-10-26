using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wordGenerator : MonoBehaviour {

    public TextAsset adj;
    public TextAsset noun;

    //public Text wordText;
    //public TextMeshProUGUI wordText;
    string[] adjList;
    string[] nounList;

    string Adj;
    string Noun;
    string myWord;



    void Start() {
        adjList = adj.text.Split("\n"[0]);
        nounList = noun.text.Split("\n"[0]);
        WordG();
        GameManager.instance.ChangeDrawText("Draw:\n" + myWord);
    }



    public void WordG() {
        Adj = (adjList[Random.Range(0, adjList.Length)]);
        Noun = (nounList[Random.Range(0, nounList.Length)]);
        myWord = Adj + " " + Noun;
        print(myWord);
    }



    void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            WordG();
            GameManager.instance.ChangeDrawText("Draw:\n" + myWord);
        }
    }
}
