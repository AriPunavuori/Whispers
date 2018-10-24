<<<<<<< HEAD:Whispers/Assets/InputManager.cs
﻿using System.Collections;
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
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    Vector3 curPos;
    public Vector3 drawPos;
    List<Vector3> line;
    LineRenderer currentLine;
    public GameObject linePrefab;
    float lineDotThreshold = 0;

    public bool IsDrawing() {
        return currentLine != null;
    }

    public static InputManager instance;



    private void Awake() {
        instance = this;
        line = new List<Vector3>();
    }

    void Update () {
        /*if(Input.GetKeyDown(KeyCode.Space)) {
            var lr = GetComponent<LineRenderer>();
            lr.positionCount = 3;
            var pos = new Vector3[] {
                Vector3.zero,
                new Vector3(1,1,0),
                new Vector3(1,0,0)
            };
            lr.SetPositions(pos);
        }*/
            if(Input.GetKeyDown(KeyCode.Mouse0)){
            curPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            curPos.z = 0;
            drawPos = curPos;
            var newLine  = Instantiate(linePrefab);
            currentLine = newLine.GetComponent<LineRenderer>();
            // Viivan ensimmäinen piste on drawPos
            line.Clear();
            line.Add(drawPos);
            currentLine.positionCount = 0;
            print("Viiva alkaa");
        }

        if(Input.GetKey(KeyCode.Mouse0)) {
            curPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            curPos.z = 0;
            //if(Vector3.Distance(curPos, drawPos) > lineDotThreshold){
                drawPos = curPos;
                line.Add(drawPos);
                currentLine.positionCount = line.Count;
                currentLine.SetPositions(line.ToArray());
                // Viivan n:s piste on on drawPos
            //}
            print("Viiva jatkuu");
        }

        if (Input.GetKeyUp(KeyCode.Mouse0)){
            curPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            curPos.z = 0;
            //if(Vector3.Distance(curPos,drawPos) > lineDotThreshold){
                drawPos = curPos;
            //}
            line.Add(drawPos);
            currentLine.positionCount = line.Count;
            currentLine.SetPositions(line.ToArray());
            DrawingMachine.instance.lines.Add(line);
            print("Viiva loppuu");
        }
    }
}
>>>>>>> c1bb3c8b019a6285f5ebe2425c63e7e2108d1db2:Whispers/Assets/Scripts/InputManager.cs
