using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Picture = System.Collections.Generic.List<LineData>;

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
    static DrawingMachine _instance;
    public static DrawingMachine instance {
        get {
            if(!_instance) {
                _instance = FindObjectOfType<DrawingMachine>();
            }
            return _instance;
        }
    }

    public List<LineData> lines;
    public List<GameObject> drawnLines;
    public Vector3 drawPos;
    LineData line;
    LineRenderer currentLine;
    public GameObject bluePrefab;
    public GameObject redPrefab;
    public int lineNumber = 0;
    float lineDotThreshold = 0.01f;
    public Drawmode mode;
    public GameObject pocket;

    //RoundDataManager rdm;
    //PlayerManager pm;
    //WordGenerator wg;
    //InputManager im;
    //GameManager gm;
    UIManager um;

    private void Awake() {
        //pm = PlayerManager.instance;
        //wg = WordGenerator.instance;
        //rdm = RoundDataManager.instance;
        //im = InputManager.instance;
        um = UIManager.instance;
        //gm = GameManager.instance;
        lines = new List<LineData>();
        drawnLines = new List<GameObject>();
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
        newLine.transform.parent = um.pocket.transform;
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
        //PrintLines();
    }

    public void ChangeColor(){
        mode = mode == Drawmode.Blue ? Drawmode.Red : Drawmode.Blue;
    }

    public void ShowPicture(Picture picture) {

        foreach (var l in picture) {
            var drawnLine = Instantiate(bluePrefab);
            drawnLine.transform.parent = um.pocket.transform;
            var lineToDraw = drawnLine.GetComponent<LineRenderer>();
            lineToDraw.positionCount = l.points.Count;
            lineToDraw.SetPositions(l.points.ToArray());
        }
    }

    public void EraseDrawnLines(){
        um.PocketReset();
        lines.Clear();
    }

    public void DeleteLastLine(){
        if(lineNumber > 0){
            var last = drawnLines[lineNumber - 1];
            drawnLines.RemoveAt(lineNumber - 1);
            lines.RemoveAt(lineNumber - 1);
            Destroy(last);
            lineNumber--;
        }
    }
}
