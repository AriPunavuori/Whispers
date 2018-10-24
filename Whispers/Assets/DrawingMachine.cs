using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingMachine : MonoBehaviour {
    public enum Drawmode { Draw, Erase };
    public static DrawingMachine instance;
    public List<List<Vector3>> lines;
    public Vector3 drawPos;
    List<Vector3> line;
    LineRenderer currentLine;
    public GameObject linePrefab;
    public GameObject eraserPrefab;
    int lineNumber = 0 ;
    float lineDotThreshold = 0.1f;
    public Drawmode mode;

    private void Awake() {
        lines = new List<List<Vector3>>();
        line = new List<Vector3>();
        instance = this;
        mode = Drawmode.Draw;
    }


    public void PrintLines(){
        //print(lines.Count);
        //for(int i = 0 ; i < lines.Count ; i++){
        //    var s = "";
        //    for(int j = 0 ; j < lines[i].Count ; j++){
        //        s += lines[i][j] + " ";
        //    }
        //    print(s);
        //}
        foreach (var l in lines) {
            var s = "";
            foreach (var coord in l) {
                s += coord + " ";
            }
            print(s);
        }
    }

    public void LineStart(Vector3 curPos) {
        drawPos = curPos;

        if (mode == Drawmode.Draw){
            var newLine = Instantiate(linePrefab);
            currentLine = newLine.GetComponent<LineRenderer>();
        } else {
            var newLine = Instantiate(eraserPrefab);
            currentLine = newLine.GetComponent<LineRenderer>();
        }

        line.Clear();
        line.Add(drawPos);
        currentLine.sortingOrder = lineNumber;
        lineNumber++;
        currentLine.positionCount = 0;
    }

    public void LineContinued(Vector3 curPos){
        if(Vector3.Distance(curPos, drawPos) > lineDotThreshold) {
            drawPos = curPos;
            line.Add(drawPos);
            currentLine.positionCount = line.Count;
            currentLine.SetPositions(line.ToArray());
        }
    }

    public void LineEnded(Vector3 curPos) {
        if(Vector3.Distance(curPos, drawPos) > lineDotThreshold) {
            drawPos = curPos;
        }
        line.Add(drawPos);

        currentLine.positionCount = line.Count;
        currentLine.SetPositions(line.ToArray());
        DrawingMachine.instance.lines.Add(new List<Vector3>(line));
        DrawingMachine.instance.PrintLines();
    }

    public void DrawingEnabler(){
        mode = Drawmode.Draw;
    }

    public void EraserEnabler(){
        mode = Drawmode.Erase;
    }

}
