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

    private void Awake() {
        hg = HostGame.instance;
        um = UIManager.instance;
    }

    private void Start() {
        for(int i = 0 ; i < hg.numberOfPlayers ; i++) {
            textToDisplay = i+1 + ". ";
            inputText.Add(textToDisplay);
        }
        um.uiText.text = textToDisplay;
    }

    // Update is called once per frame
    void Update () {

	}

    public void UpdateInputTextList(){
        print(hg.numberOfPlayers);
        textToDisplay = "";
        var player = FindObjectOfType<PlayerConnectionObject>();
        var id = player.netId.Value;
        print(id);
        inputText.Insert((int)id-1, textField.text);
        for(int i = 0 ; i < inputText.Count ; i++ ){
            textToDisplay = textToDisplay + i + inputText[i] + "\n";
        }
        um.uiText.text = textToDisplay;
    }
}