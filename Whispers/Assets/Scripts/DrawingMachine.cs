using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Picture = System.Collections.Generic.List<LineData>;

[System.Serializable]
public struct LineData {

    public Vector3[] points;

    public LineData(Vector3[] points) {
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
    List<Vector3> line;
    LineRenderer currentLine;
    public GameObject linePrefab;
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
        line = new List<Vector3>();
        GameObject newLine = Instantiate(linePrefab);
        newLine.transform.parent = um.pocket.transform;
        currentLine = newLine.GetComponent<LineRenderer>();
        drawnLines.Add(newLine);
        line.Clear();
        line.Add(drawPos);
        currentLine.sortingOrder = lineNumber;
        lineNumber++;
        currentLine.positionCount = 0;
    }

    public void LineContinued(Vector3 curPos) {
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
        lines.Add(new LineData(line.ToArray()));
        //PrintLines();
    }

    public void DeleteLastLine(){
        print(lineNumber);
        if(lineNumber > 0){
            var last = drawnLines[lineNumber - 1];
            drawnLines.RemoveAt(lineNumber - 1);
            lines.RemoveAt(lineNumber - 1);
            Destroy(last);
            lineNumber--;
        }
    }
}
