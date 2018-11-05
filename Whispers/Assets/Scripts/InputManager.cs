using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

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

    RoundDataManager rdm;
    PlayerManager pm;
    DrawingMachine dm;
    UIManager um;
    GameManager gm;
    SmallTalkGenerator smg;

    private void Awake() {
        rdm = RoundDataManager.instance;
        pm = PlayerManager.instance;
        um = UIManager.instance;
        dm = DrawingMachine.instance;
        gm = GameManager.instance;
        smg = SmallTalkGenerator.instance;
    }

    void Update() {

        // Kosketuksen alussa tehdään seuraavaa
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
        dm.DeleteLastLine();
    }

    public void SendDrawing() { // Tallennetaan kuva
        gm.timerTime = 0;
        rdm.AddPictureToChain(dm.lines, 0);
    }

    public void SendGuess() { // Funktio joka kutsutaan UI-Buttonilla kirjoitus-UI:ssä
        if(!gm.nameSet){
            pm.SetPlayerName(um.textBox.text);
            smg.Jones = um.textBox.text;
        } else{
            rdm.guess = um.textBox.text;
            um.textBox.text = "";
            gm.timerTime = 0;
            rdm.AddGuessToChain(rdm.guess, pm.playerData.playerID);
        }
    }

    private bool IsPointerOverUIObject(Vector2 position) { // Onko input UI-Elementtien päällä?
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = position;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}