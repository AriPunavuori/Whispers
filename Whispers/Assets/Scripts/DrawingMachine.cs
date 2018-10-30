using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LineData {
    public bool isBlue;
    public List<Vector3> points;

    public LineData(bool isBlue, List<Vector3> points) {
        this.isBlue = isBlue;
        this.points = points;
    }
}

public class DrawingMachine : MonoBehaviour {
    public enum Drawmode { Blue, Red };
    public static DrawingMachine instance;
    public List<LineData> lines;
    public List<GameObject> drawnLines;
    public Vector3 drawPos;
    LineData line;
    LineRenderer currentLine;
    public GameObject bluePrefab;
    public GameObject redPrefab;
    int lineNumber = 0;
    float lineDotThreshold = 0.01f;
    public Drawmode mode;
    public GameObject pocket;

    private void Awake() {
        lines = new List<LineData>();
        drawnLines = new List<GameObject>();
        instance = this;
        mode = Drawmode.Blue;
    }

    public void PrintLines() {
    
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
        line = new LineData(true, new List<Vector3>());
        GameObject newLine = Instantiate(mode == Drawmode.Blue ? bluePrefab : redPrefab);
        newLine.transform.parent = GameManager.instance.pocket.transform;
        currentLine = newLine.GetComponent<LineRenderer>();
        drawnLines.Add(newLine);
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
        lines.Add(line);
        PrintLines();
    }

    public void ChangeColor(){
        mode = mode == Drawmode.Blue ? Drawmode.Red : Drawmode.Blue;
    }

    //public void ShowDrawnLines(){

    //    print(lines.Count);
    //    foreach(var l in lines) {

    //        var drawnLine = Instantiate(bluePrefab); // Fiksaa värit
    //        drawnLine.transform.parent = GameManager.instance.pocket.transform;

    //        var lineToDraw = drawnLine.GetComponent<LineRenderer>();
    //        foreach(var point in l.points) {
    //            lineToDraw.positionCount = l.points.Count;
    //            lineToDraw.SetPositions(l.points.ToArray());
    //        }
    //    }
    //}

    public void EraseDrawnLines(){
        GameManager.instance.PocketReset();
        lines.Clear();
    }

    public void Undo(){
        if(lineNumber > 0){
            var last = drawnLines[lineNumber - 1];
            drawnLines.RemoveAt(lineNumber - 1);
            Destroy(last);
            lineNumber--;
        }
    }
}
