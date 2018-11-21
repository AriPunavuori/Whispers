using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using Picture = System.Collections.Generic.List<LineData>;

public class UIManager : NetworkBehaviour {

    //static UIManager _instance;
    //public static UIManager instance{
    //    get {
    //        if(!_instance) {
    //            _instance = FindObjectOfType<UIManager>();
    //        }
    //        return _instance;
    //    }
    //}

    public TextMeshProUGUI uiText;
    public TextMeshProUGUI roomCodeTxt;

    //RoundDataManager rdm;
    //PlayerManager pm;
    //DrawingMachine dm;
    //HostGame hg;
    //InputManager im;
    //GameManager gm;

    public GameObject drawingUI;
    public GameObject waitingUI;
    public GameObject watchingUI;
    public GameObject writingUI;

    public GameObject pocket;
    public GameObject pocketPrefab;

    public GameObject linePrefab;
    public GameObject startButton;
    public GameObject rdmPrefab;
    public InputField textBox;

    public GameObject paperBCG;

    private void Awake() {
        //rdm = RoundDataManager.instance;
        //gm = GameManager.instance;
        //pm = PlayerManager.instance;
        //dm = DrawingMachine.instance;
        //hg = HostGame.instance;

        //wg = WordGenerator.instance;
        //im = InputManager.instance;
    }

    private void Start() {
        PocketReset();
        var pm = FindObjectOfType<PlayerManager>();
        if(pm.playerData.playerIsHost) {
            startButton.gameObject.SetActive(true);
        } else {
            startButton.gameObject.SetActive(false);
        }
    }

    public void ChangeUIText(string text) { // UI-Tekstin vaihto
        uiText.text = text;
    }

    public void SetUI() { // Vaihdetaan UI-Näkymää
        var pm = FindObjectOfType<PlayerManager>();
        drawingUI.SetActive(pm.playMode == PlayerManager.PlayMode.Draw);
        waitingUI.SetActive(pm.playMode == PlayerManager.PlayMode.Wait);
        watchingUI.SetActive(pm.playMode == PlayerManager.PlayMode.Watch);
        writingUI.SetActive(pm.playMode == PlayerManager.PlayMode.Write);
        if (pm.playMode == PlayerManager.PlayMode.Draw || pm.playMode == PlayerManager.PlayMode.Write) {
            paperBCG.SetActive(true);
        } else {
            paperBCG.SetActive(false);
        }
    }

    

    public void ShowPictureToGuess() { // Näytetään kuva arvattavaksi
        var rdm = FindObjectOfType<RoundDataManager>();
        var gm = FindObjectOfType<GameManager>();
        var pm = FindObjectOfType<PlayerManager>();
        var hg = FindObjectOfType<HostGame>();
        var chainIdx = (gm.roundNumbr + pm.playerData.playerID) % hg.numberOfPlayers;
        var pics = rdm.chains[chainIdx].pictures;
        print("Näytetään kuva ketjusta:" + chainIdx);
        ShowPicture(pics[(gm.roundNumbr - 1) / 2]);
    }

    public void ShowPicture(LineData[] picture) {

        foreach(var l in picture) {
            var drawnLine = Instantiate(linePrefab);
            drawnLine.transform.parent = pocket.transform;
            var lineToDraw = drawnLine.GetComponent<LineRenderer>();
            lineToDraw.positionCount = l.points.Length;
            lineToDraw.SetPositions(l.points);
        }
    }

    public void ShowTextToDraw() { // Näytetään teksti piirrettäväksi
        var rdm = FindObjectOfType<RoundDataManager>();
        var gm = FindObjectOfType<GameManager>();
        var hg = FindObjectOfType<HostGame>();
        var pm = FindObjectOfType<PlayerManager>();
        var chainIdx = (gm.roundNumbr + pm.playerData.playerID) % hg.numberOfPlayers;
        print("Näytetään teksti ketjusta: " + chainIdx);
        var temp = gm.roundNumbr - 1;
        print(uiText);
        print(rdm);
        //print(rdm.chains[chainIdx].guesses[0]);
        if (temp == 0)
            ChangeUIText("Draw " + rdm.chains[chainIdx].guesses[0]);
        else
            ChangeUIText("Draw " + rdm.chains[chainIdx].guesses[(gm.roundNumbr - 1) / 2]);

    }

    public void EraseDrawnLines() {
        var dm = FindObjectOfType<DrawingMachine>();
        PocketReset();
        dm.lines.Clear();
        dm.drawnLines.Clear();
        dm.lineNumber = 0;
    }

    public void PocketReset() { // Piirrettyjen viivojen(Peliobjektien) poisto 
        Destroy(pocket);
        pocket = Instantiate(pocketPrefab);
    }

    [Command]
    public void CmdCreateRdmOnHost() {
        startButton.gameObject.SetActive(false);
        RpcCreateRdmOnCLients();
    }

    [ClientRpc]
    void RpcCreateRdmOnCLients() {
        Instantiate(rdmPrefab);
        var gm = FindObjectOfType<GameManager>();
        gm.GenerateNewWordsToDraw();
    }
}
