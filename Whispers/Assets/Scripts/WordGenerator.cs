﻿using System.Collections;
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
    public string myWord;

    private void Awake() {
        instance = this;
    }

    void Start() {
        adjList = adj.text.Split((string[])null, System.StringSplitOptions.RemoveEmptyEntries);
        nounList = noun.text.Split((string[])null, System.StringSplitOptions.RemoveEmptyEntries);
    }

    public void WordG() { // Sanageneraattori
        Adj = (adjList[Random.Range(0, adjList.Length)]);
        Noun = (nounList[Random.Range(0, nounList.Length)]);
        myWord = Adj + " " + Noun;
        print(Adj.Length);
        GameManager.instance.ChangeUIText("Hi " + PlayerManager.instance.playerData.playerName + ", Can you draw:\n" + myWord + "?");
    }
}
