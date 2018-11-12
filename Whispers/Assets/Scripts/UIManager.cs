using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using Picture = System.Collections.Generic.List<LineData>;

public class UIManager : NetworkBehaviour {

    static UIManager _instance;
    public static UIManager instance{
        get {
            if(!_instance) {
                _instance = FindObjectOfType<UIManager>();
            }
            return _instance;
        }
    }

    public TextMeshProUGUI uiText;

    RoundDataManager rdm;
    PlayerManager pm;
    DrawingMachine dm;
    HostGame hg;
    //InputManager im;
    GameManager gm;

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

    private void Awake() {
        rdm = RoundDataManager.instance;
        gm = GameManager.instance;
        pm = PlayerManager.instance;
        dm = DrawingMachine.instance;
        hg = HostGame.instance;

        //wg = WordGenerator.instance;
        //im = InputManager.instance;
    }

    private void Start() {
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
        drawingUI.SetActive(pm.playMode == PlayerManager.PlayMode.Draw);
        waitingUI.SetActive(pm.playMode == PlayerManager.PlayMode.Wait);
        watchingUI.SetActive(pm.playMode == PlayerManager.PlayMode.Watch);
        writingUI.SetActive(pm.playMode == PlayerManager.PlayMode.Write);
    }

    public void ShowPictureToGuess(int playerID) { // Näytetään kuva arvattavaksi
        var chainIdx = (gm.roundNumbr + playerID - 1) % hg.numberOfPlayers;
        var pics = rdm.chains[chainIdx].pictures;
        ShowPicture(pics[gm.roundNumbr / 2]);
    }

    public void ShowPicture(Picture picture) {

        foreach(var l in picture) {
            var drawnLine = Instantiate(linePrefab);
            drawnLine.transform.parent = pocket.transform;
            var lineToDraw = drawnLine.GetComponent<LineRenderer>();
            lineToDraw.positionCount = l.points.Count;
            lineToDraw.SetPositions(l.points.ToArray());
        }
    }

    public void ShowTextToDraw() { // Näytetään teksti piirrettäväksi
        ChangeUIText("Draw " + rdm.guess);
    }

    public void EraseDrawnLines() {
        PocketReset();
        dm.lines.Clear();
        dm.drawnLines.Clear();
        dm.lineNumber = 0;
    }

    public void PocketReset() { // Piirrettyjen viivojen(Peliobjektien) poisto 
        Destroy(pocket);
        pocket = Instantiate(pocketPrefab);
    }

    public void Click(){
        CmdCreateRdmOnHost();
    }

    [Command]
    public void CmdCreateRdmOnHost() {
        RpcCreateRdmOnCLients();
    }

    [ClientRpc]
    void RpcCreateRdmOnCLients() {
        Instantiate(rdmPrefab);
        gm.allPlayersReady = true;
    }
}
