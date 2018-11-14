using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class RoundDataManager : NetworkBehaviour {

    [System.Serializable]
    public struct ChainData {
        // Kierroksen aikana näihin tulee piirustus- ja kirjotusdatat pelaajilta.
        public List<LineData[]> pictures;
        public List<string> guesses;

        public ChainData(List<LineData[]> pictures, List<string> guesses) {
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
    HostGame hg;

    void Awake() {
        //pm = PlayerManager.instance;
        //dm = DrawingMachine.instance;
        //wg = WordGenerator.instance;
        //im = InputManager.instance;
        hg = HostGame.instance;
        gm = GameManager.instance;
        InitGame();
    }

    void InitGame() {
        for(int i = 0 ; i < hg.numberOfPlayers ; i++) {
            chains.Add(new ChainData(new List<LineData[]>(), new List<string>()));
        }
    }

    //[Command]
    //public void CmdAddPictureToChain(LineData[] picture, int chainID) {
    //    chains[chainID].pictures.Add(picture);
    //}

    //[Command]
    //public void CmdAddGuessToChain(string text, int chainID) {
    //    chains[chainID].guesses.Add(text.RemoveDiacritics());
    //    //print(guesses[gm.roundNumbr/2]);
    //}

    [ClientRpc]
    public void RpcDistributeChainData() {

    }

}
