using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class InputTest : NetworkBehaviour {

    public string textToDisplay = "";
    //[SyncVar]
    public List<string> inputText = new List<string>();
    public InputField textField;
    HostGame hg;
    UIManager um;
    PlayerManager pm;

    private void Awake() {
        hg = HostGame.instance;
        um = UIManager.instance;
        pm = PlayerManager.instance;
    }

    private void Start() {
        print(hg.numberOfPlayers);
        for(int i = 0 ; i < 6 ; i++) {
            inputText.Add(i + 1 + "." + "\n");
            print(inputText[i]);
            textToDisplay = textToDisplay + inputText[i];
        }
        um.uiText.text = textToDisplay;
    }

    // Update is called once per frame
    void Update () {

	}



    public void UpdateInputTextList(){
        //print(hg.numberOfPlayers);
        textToDisplay = "";
        print(pm.playerData.playerID);
        print(inputText.Count);
        inputText[pm.playerData.playerID] = textField.text;
        for(int i = 0 ; i < inputText.Count ; i++ ){
            if(i==pm.playerData.playerID){
                textToDisplay = textToDisplay + (i + 1) + ". " + inputText[i] + "\n";
            } else{
                textToDisplay = textToDisplay + inputText[i];
            }
        }
        um.uiText.text = textToDisplay;
    }
}