using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGenerator : MonoBehaviour {
    public static WordGenerator instance;
    public TextAsset adj;
    public TextAsset noun;

    //public Text wordText;
    //public TextMeshProUGUI wordText;
    string[] adjList;
    string[] nounList;

    string Adj;
    string Noun;
    string myWord;

    private void Awake() {
        instance = this;
    }

    void Start() {
        adjList = adj.text.Split("\n"[0]);
        nounList = noun.text.Split("\n"[0]);
    }

    public void WordG() {
        Adj = (adjList[Random.Range(0, adjList.Length)]);
        Noun = (nounList[Random.Range(0, nounList.Length)]);
        myWord = Adj + " " + Noun;
        GameManager.instance.ChangeDrawText("Hi " + PlayerManager.instance.playerData.playerName + ", Can you draw:\n" + myWord + "?");
    }
}
