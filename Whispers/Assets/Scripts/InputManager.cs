using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static InputManager instance;

    private void Awake() {
        instance = this;
    }

    void Update() {

        if(Input.GetKeyDown(KeyCode.Mouse0)|| Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            DrawingMachine.instance.LineStart(pos);
        }

        if(Input.GetKey(KeyCode.Mouse0)|| Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            DrawingMachine.instance.LineContinued(pos);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0)|| Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            DrawingMachine.instance.LineEnded(pos);
        }

        if (Input.GetKeyDown(KeyCode.Space)){
            if(DrawingMachine.instance.mode == DrawingMachine.Drawmode.Draw){
                DrawingMachine.instance.DrawingEnabler();
            } else {
                DrawingMachine.instance.EraserEnabler();
            }
            
        }

       

    }
}