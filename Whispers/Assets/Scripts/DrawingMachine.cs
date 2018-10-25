using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LineData {
    public bool isBlack;
    public List<Vector3> points;

    public LineData(bool isBlack, List<Vector3> points) {
        this.isBlack = isBlack;
        this.points = points;
    }
}

public class DrawingMachine : MonoBehaviour {
    public enum Drawmode { Draw, Erase };
    public static DrawingMachine instance;
    public List<LineData> lines;
    public List<GameObject> drawedLines;
    public Vector3 drawPos;
    LineData line;
    LineRenderer currentLine;
    public GameObject linePrefab;
    public GameObject eraserPrefab;
    int lineNumber = 0;
    float lineDotThreshold = 0.1f;
    public Drawmode mode;

    private void Awake() {
        lines = new List<LineData>();
        line = new LineData(true, new List<Vector3>());
        drawedLines = new List<GameObject>();
        instance = this;
        mode = Drawmode.Draw;
    }


    public void PrintLines() {
        //print(lines.Count);
        //for(int i = 0 ; i < lines.Count ; i++){
        //    var s = "";
        //    for(int j = 0 ; j < lines[i].Count ; j++){
        //        s += lines[i][j] + " ";
        //    }
        //    print(s);
        //}
        foreach(var l in lines) {
            var s = "";
            foreach(var coord in l.points) {
                s += coord + " ";
            }
            print(s);
        }
    }

    public void LineStart(Vector3 curPos) {
        drawPos = curPos;

        GameObject newLine = Instantiate(mode == Drawmode.Draw ?
                                         linePrefab :
                                         eraserPrefab);

        //if(mode == Drawmode.Draw) {
        //    newLine = Instantiate(linePrefab);
        //} else {
        //    newLine = Instantiate(eraserPrefab);
        //}
        currentLine = newLine.GetComponent<LineRenderer>();
        drawedLines.Add(newLine);

        line.points.Clear();
        line.points.Add(drawPos);
        currentLine.sortingOrder = lineNumber;
   
        lineNumber++;
        currentLine.positionCount = 0;
    }

    public void LineContinued(Vector3 curPos) {
        if(Vector3.Distance(curPos, drawPos) > lineDotThreshold) {
            drawPos = curPos;
            line.points.Add(drawPos);
            currentLine.positionCount = line.points.Count;
            currentLine.SetPositions(line.points.ToArray());
        }
    }

    public void LineEnded(Vector3 curPos) {
        if(Vector3.Distance(curPos, drawPos) > lineDotThreshold) {
            drawPos = curPos;
        }
        line.points.Add(drawPos);

        currentLine.positionCount = line.points.Count;
        currentLine.SetPositions(line.points.ToArray());
        //DrawingMachine.instance.lines.Add(new(line.points));
        DrawingMachine.instance.PrintLines();
    }

    public void DrawingEnabler() {
        mode = Drawmode.Draw;
    }

    public void EraserEnabler() {
        mode = Drawmode.Erase;
    }

    public void Undo(){
        if(lineNumber > 0){
            var last = drawedLines[lineNumber - 1];
            drawedLines.RemoveAt(lineNumber - 1);
            Destroy(last);
            lineNumber--;
        }
    }
}
