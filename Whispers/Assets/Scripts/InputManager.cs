using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class InputManager : NetworkBehaviour {

    static InputManager _instance;
    public static InputManager instance {
        get {
            if(!_instance) {
                _instance = FindObjectOfType<InputManager>();
            }
            return _instance;
        }
    }
    public bool isDrawing = false;

    //RoundDataManager rdm;
    //PlayerManager pm;
    //DrawingMachine dm;
    //UIManager um;
    //GameManager gm;
    //HostGame hg;
    //WordGenerator wg;

    private void Awake() {
        //wg = WordGenerator.instance;
        //rdm = RoundDataManager.instance;
        //pm = PlayerManager.instance;
        //um = UIManager.instance;
        //dm = DrawingMachine.instance;
        //gm = GameManager.instance;
        //hg = HostGame.instance;
    }

    void Update() {

        // Kosketuksen alussa tehdään seuraavaa
        var pm = FindObjectOfType<PlayerManager>();
        var um = FindObjectOfType<UIManager>();
        var dm = FindObjectOfType<DrawingMachine>();
        if(!IsPointerOverUIObject(Input.mousePosition) && pm.playMode == PlayerManager.PlayMode.Draw) {
            if(Input.GetKeyDown(KeyCode.Mouse0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                dm.LineStart(pos);
                isDrawing = true;
            }
        }
       
        if(isDrawing) {
            // Kosketuksen jatkuessa
            if(Input.GetKey(KeyCode.Mouse0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && pm.playMode == PlayerManager.PlayMode.Draw) {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                dm.LineContinued(pos);
            }
            // Kosketuksen päättyessä
            if(Input.GetKeyUp(KeyCode.Mouse0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && pm.playMode == PlayerManager.PlayMode.Draw) {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                dm.LineEnded(pos);
                isDrawing = false;
            }
        }
    }

    public void Undo(){
        var dm = FindObjectOfType<DrawingMachine>();
        dm.DeleteLastLine();
    }

    public void SendDrawing() { // Tallennetaan kuva
        var pco = FindObjectOfType<PlayerConnectionObject>();
        var pm = FindObjectOfType<PlayerManager>();
        var gm = FindObjectOfType<GameManager>();
        var dm = FindObjectOfType<DrawingMachine>();
        var hg = FindObjectOfType<HostGame>();
        var um = FindObjectOfType<UIManager>();
        print("Lisätään kuva ketjuun: " + (pm.playerData.playerID + gm.roundNumbr) % hg.numberOfPlayers);
        pco.CmdAddPictureToChain(dm.lines.ToArray(), (pm.playerData.playerID + gm.roundNumbr) % hg.numberOfPlayers);
        pm.playMode = PlayerManager.PlayMode.Wait;
        um.SetUI();
        um.EraseDrawnLines();
        um.ChangeUIText("");
        pco.CmdThisClientIsReady();
        pm.playerData.playerRDY = true;
    }

    public void SendGuess() { // Funktio joka kutsutaan UI-Buttonilla kirjoitus-UI:ssä
        var pco = FindObjectOfType<PlayerConnectionObject>();
        var rdm = FindObjectOfType<RoundDataManager>();
        var pm = FindObjectOfType<PlayerManager>();
        var gm = FindObjectOfType<GameManager>();
        var dm = FindObjectOfType<DrawingMachine>();
        var hg = FindObjectOfType<HostGame>();
        var um = FindObjectOfType<UIManager>();

        rdm.guess = um.textBox.text;
        um.textBox.text = "";
        print("Lisätään teksti ketjuun: " + (pm.playerData.playerID + gm.roundNumbr) % hg.numberOfPlayers);
        pco.CmdAddGuessToChain(rdm.guess, (pm.playerData.playerID + gm.roundNumbr) % hg.numberOfPlayers);
        pm.playMode = PlayerManager.PlayMode.Wait;
        um.SetUI();
        um.PocketReset();
        um.ChangeUIText("");
        pco.CmdThisClientIsReady();
        pm.playerData.playerRDY = true;
    }

    public void CreateRoom(){
        var pco = FindObjectOfType<PlayerConnectionObject>();
        var pm = FindObjectOfType<PlayerManager>();
        var gm = FindObjectOfType<GameManager>();
        var dm = FindObjectOfType<DrawingMachine>();
        var hg = FindObjectOfType<HostGame>();
        var um = FindObjectOfType<UIManager>();
        hg.CreateRoom();
        pm.playMode = PlayerManager.PlayMode.Wait;
        um.SetUI();
        //um.uiText.text = "Lobby:\nAsk ppl to join room #: " + hg.roomCode;
    }

    public void JoinRoom(){

    }

    private bool IsPointerOverUIObject(Vector2 position) { // Onko input UI-Elementtien päällä?
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = position;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}