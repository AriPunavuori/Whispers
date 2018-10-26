using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

    public static InputManager instance;
    public bool isDrawing = false;
    private void Awake() {
        instance = this;
    }

    void Update() {

        if(!IsPointerOverUIObject(Input.mousePosition)) {
            if(Input.GetKeyDown(KeyCode.Mouse0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                DrawingMachine.instance.LineStart(pos);
                isDrawing = true;
            }
        }

        if(isDrawing){
            if(Input.GetKey(KeyCode.Mouse0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                DrawingMachine.instance.LineContinued(pos);
            }
            if(Input.GetKeyUp(KeyCode.Mouse0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                DrawingMachine.instance.LineEnded(pos);
                isDrawing = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.Space)) {
            DrawingMachine.instance.Undo();
            //if(DrawingMachine.instance.mode == DrawingMachine.Drawmode.Draw){
            //    DrawingMachine.instance.DrawingEnabler();
            //} else {
            //    DrawingMachine.instance.EraserEnabler();
            //}
        }
    }

    private bool IsPointerOverUIObject(Vector2 position) {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = position;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}