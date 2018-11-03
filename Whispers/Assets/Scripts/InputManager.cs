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

    //RoundDataManager rdm;
    //PlayerManager pm;
    DrawingMachine dm;
    //WordGenerator wg;
    GameManager gm;

    private void Awake() {
        //rdm = RoundDataManager.instance;
        //pm = PlayerManager.instance;
        //wg = WordGenerator.instance;
        dm = DrawingMachine.instance;
        gm = GameManager.instance;
    }
    void Update() {

        // Kosketuksen alussa tehdään seuraavaa
        if(!IsPointerOverUIObject(Input.mousePosition) && gm.mode == GameManager.PlayerMode.Draw) {
            if(Input.GetKeyDown(KeyCode.Mouse0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                dm.LineStart(pos);
                isDrawing = true;
            }
        }
       
        if(isDrawing) {
            // Kosketuksen jatkuessa
            if(Input.GetKey(KeyCode.Mouse0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && gm.mode == GameManager.PlayerMode.Draw) {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                dm.LineContinued(pos);
            }
            // Kosketuksen päättyessä
            if(Input.GetKeyUp(KeyCode.Mouse0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended&& gm.mode == GameManager.PlayerMode.Draw) {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                dm.LineEnded(pos);
                isDrawing = false;
            }
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