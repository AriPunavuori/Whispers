using System.Collections;
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
