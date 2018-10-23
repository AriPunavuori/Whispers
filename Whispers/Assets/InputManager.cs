using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    Vector3 curPos;
    public Vector3 drawPos;
    public bool drawing = false;
    bool drawingEnded = false;
 
    float lineDotThreshold = 1;

    public static InputManager instance;

    private void Awake() {
        instance = this;
    }

    void Update () {

        if(drawingEnded){
            drawing = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray)){
                curPos = Input.mousePosition;
                drawPos = Input.mousePosition;
                drawing = true;
                drawingEnded = false;
                // Viivan ensimmäinen piste on drawPos
            }
        }

        if(Input.GetKey(KeyCode.Mouse0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray)) {
                curPos = Input.mousePosition;
                if(Vector3.Distance(curPos, drawPos) > lineDotThreshold){
                    drawPos = curPos;
                    // Viivan n:s piste on on drawPos
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray)){
                curPos = Input.mousePosition;
                if(Vector3.Distance(curPos,drawPos) > lineDotThreshold){
                    drawPos = curPos;
                }
                // Viivan päätepiste on drawPos;
            }
            drawingEnded = true;
        }

    }
}
