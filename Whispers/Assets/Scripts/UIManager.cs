using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Picture = System.Collections.Generic.List<LineData>;

public class UIManager : MonoBehaviour {

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
    //PlayerManager pm;
    //DrawingMachine dm;
    //WordGenerator wg;
    //InputManager im;
    GameManager gm;

    public GameObject drawingUI;
    public GameObject menuUI;
    public GameObject waitingUI;
    public GameObject watchingUI;
    public GameObject writingUI;

    public GameObject pocket;
    public GameObject pocketPrefab;
    public GameObject linePrefab;

    public InputField textBox;

    private void Awake() {
        rdm = RoundDataManager.instance;
        gm = GameManager.instance;
        //pm = PlayerManager.instance;
        //dm = DrawingMachine.instance;
        //wg = WordGenerator.instance;
        //im = InputManager.instance;
    }

    public void ChangeUIText(string text) { // UI-Tekstin vaihto
        uiText.text = text;
    }

    public void SetUI(bool d) { // Vaihdetaan UI-Näkymää
        drawingUI.SetActive(d);
        //menuUI.SetActive(d);
        //waitingUI.SetActive(d);
        //watchingUI.SetActive(d);
        writingUI.SetActive(!d);
    }

    public void ShowPictureToGuess(int playerID) { // Näytetään kuva arvattavaksi
        var chainIdx = (gm.roundNumbr + playerID - 1) % gm.playerCount;
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

    public void PocketReset() { // Piirrettyjen viivojen(Peliobjektien) poisto 
        Destroy(pocket);
        pocket = Instantiate(pocketPrefab);
    }

}
