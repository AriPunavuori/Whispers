using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public struct PlayerData {
    public string playerName;
    public bool playerRDY;
    public float playerIMG;
    public bool playerIsHost;
    public int playerID;

    public PlayerData(string playerName, bool playerRDY, float playerIMG, bool playerIsHost, int playerID) {
        this.playerName = playerName;
        this.playerRDY = playerRDY;
        this.playerIMG = playerIMG;
        this.playerIsHost = playerIsHost;
        this.playerID = playerID;
    }
}

public class PlayerManager : MonoBehaviour {
    public enum PlayMode { Draw, Write, Wait, Watch, Menu };
    public PlayMode playMode;

    public InputField nameInput;
    public GameObject textContainer;
    public Text statusText;
    public UnityEvent onValidName;

    //static PlayerManager _instance;
    //public static PlayerManager instance{
    //    get{
    //        if(!_instance){
    //            _instance = FindObjectOfType<PlayerManager>();
    //        }
    //        return _instance;
    //    }
    //}

    bool created;
    public PlayerData playerData;
    public List<PlayerData> ServersPlayerDataList;
    public List<PlayerData> playerDataList;

    //RoundDataManager rdm;
    //WordGenerator wg;
    //InputManager im;
    //DrawingMachine dm;
    //GameManager gm;

    private void Awake() { 

        if(!created) {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        //PlayerPrefs.SetString("Name", ""); // Tällä voi nollata nimen
        //if(PlayerPrefs.GetString("Name") == null){
        //    playerData.playerName = "";
        //} else {
        //    playerData.playerName = PlayerPrefs.GetString("Name");
        //}


        //rdm = RoundDataManager.instance;
        //wg = WordGenerator.instance;
        //im = InputManager.instance;
        //dm = DrawingMachine.instance;
        //gm = GameManager.instance;
        playMode = PlayMode.Menu;
    }

    public void SetName() {
        if (nameInput.text == "") {
            // show errormsg window and ask new name
            textContainer.SetActive(true);
            textContainer.gameObject.GetComponentInChildren<SVGImage>().color = Color.red;
            Fabric.EventManager.Instance.PostEvent("error2");
            statusText.text = "No name set\n Please set new name";
            return;
        }
        playerData.playerName = nameInput.text.RemoveDiacritics();
        Fabric.EventManager.Instance.PostEvent("button2");
        onValidName.Invoke();
        textContainer.gameObject.GetComponentInChildren<SVGImage>().color = Color.white;

    }
}
