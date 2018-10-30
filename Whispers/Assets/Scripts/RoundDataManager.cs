using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundDataManager : MonoBehaviour {

    [System.Serializable]
    public struct ChainData {
        // Kierroksen aikana näihin tulee piirustus- ja kirjotusdatat pelaajilta.
        public List<List<LineData>> pictures;
        public List<string> guesses;

        public ChainData(List<List<LineData>> pictures, List<string> guesses) {
            this.pictures = pictures;
            this.guesses = guesses;
        }
}

    public static RoundDataManager instance;
    [SerializeField]
    //public List<List<LineData>> pictures;
    //public List<string> guesses;
    public List<ChainData> chains;
    public GameObject bluePrefab;


    void Awake() {
        instance = this;
        //pictures = new List<List<LineData>>();
        //guesses = new List<string>();
        InitGame();
    }

    void InitGame() {

    }

    public void ShowDrawnLines(List<LineData> picture) {

        print(picture.Count);
        foreach (var l in picture) {

            var drawnLine = Instantiate(bluePrefab);
            drawnLine.transform.parent = GameManager.instance.pocket.transform;

            var lineToDraw = drawnLine.GetComponent<LineRenderer>();
            //foreach (var point in l.points) {
                lineToDraw.positionCount = l.points.Count;
                lineToDraw.SetPositions(l.points.ToArray());
            //}
        }
    }

    public void AddPicture(List<LineData> picture, int playerID) {
//        pictures.Add(DrawingMachine.instance.lines);
    }

    public void AddGuess(string text, int playerID) {
        //guesses.Add(text);
        //print(guesses[GameManager.instance.roundNumbr/2]);
        
    }

}
