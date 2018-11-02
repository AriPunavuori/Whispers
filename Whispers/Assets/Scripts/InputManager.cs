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

    void Update() {

        // Kosketuksen alussa tehdään seuraavaa
        if(!IsPointerOverUIObject(Input.mousePosition) && GameManager.instance.mode == GameManager.PlayerMode.Draw) {
            if(Input.GetKeyDown(KeyCode.Mouse0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                DrawingMachine.instance.LineStart(pos);
                isDrawing = true;
            }
        }
       
        if(isDrawing) {
            // Kosketuksen jatkuessa
            if(Input.GetKey(KeyCode.Mouse0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && GameManager.instance.mode == GameManager.PlayerMode.Draw) {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                DrawingMachine.instance.LineContinued(pos);
            }
            // Kosketuksen päättyessä
            if(Input.GetKeyUp(KeyCode.Mouse0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended&& GameManager.instance.mode == GameManager.PlayerMode.Draw) {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                DrawingMachine.instance.LineEnded(pos);
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