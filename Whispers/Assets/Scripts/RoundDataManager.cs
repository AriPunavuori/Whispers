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

    public string guess;
    [SerializeField]
    public List<ChainData> chains;
    public GameObject bluePrefab;

    //PlayerManager pm;
    //DrawingMachine dm;
    //WordGenerator wg;
    //InputManager im;
    GameManager gm;

    void Awake() {
        //pm = PlayerManager.instance;
        //dm = DrawingMachine.instance;
        //wg = WordGenerator.instance;
        //im = InputManager.instance;
        gm = GameManager.instance;
        InitGame();
    }

    void InitGame() {
        for(int i = 0 ; i < gm.playerCount ; i++) {
            chains.Add(new ChainData(new List<Picture>(), new List<string>()));
        }
    }

    public void AddPictureToChain(Picture picture, int playerID) {
        //print("Player ID: " + playerID);
        print("Number of lines: " + picture.Count);
        chains[playerID].pictures.Add(picture);
    }

    public void AddGuessToChain(string text, int playerID) {
        chains[playerID].guesses.Add(text);
        //print(guesses[gm.roundNumbr/2]);
        
    }

}
