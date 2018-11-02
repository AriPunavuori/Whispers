using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Picture = System.Collections.Generic.List<LineData>;

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

    static RoundDataManager _instance;
    public static RoundDataManager instance {
        get {
            if(!_instance)
                _instance = FindObjectOfType<RoundDataManager>();
            return _instance;
        }
    }

    [SerializeField]
    public List<string> guesses;
    [SerializeField]
    public List<ChainData> chains;
    public GameObject bluePrefab;

    void Awake() {
        InitGame();
    }

    void InitGame() {
        for(int i = 0 ; i < GameManager.instance.playerCount ; i++) {
            chains.Add(new ChainData(new List<Picture>(), new List<string>()));
        }
        guesses = new List<string>();
    }

    public void ShowPicture(Picture picture) {

        foreach (var l in picture) {
            var drawnLine = Instantiate(bluePrefab);
            drawnLine.transform.parent = GameManager.instance.pocket.transform;
            var lineToDraw = drawnLine.GetComponent<LineRenderer>();
            lineToDraw.positionCount = l.points.Count;
            lineToDraw.SetPositions(l.points.ToArray());
        }
    }

    public void AddPicture(Picture picture, int playerID) {
        //print("Player ID: " + playerID);
        print("Number of lines: " + picture.Count);
        chains[playerID].pictures.Add(picture);
    }

    public void AddGuess(string text, int playerID) {
        chains[playerID].guesses.Add(text);
        //print(guesses[GameManager.instance.roundNumbr/2]);
        
    }

}
