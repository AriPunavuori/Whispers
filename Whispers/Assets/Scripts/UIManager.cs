using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public GameObject drawingUI;
    public GameObject menuUI;
    public GameObject waitingUI;
    public GameObject watchingUI;
    public GameObject writingUI;

    public void ChangeUIText(string text) { // UI-Tekstin vaihto
        uiText.text = text;
    }
    public void SetUI(bool d) { // Vaihdetaan UI-Näkymää
        drawingUI.SetActive(d);
        writingUI.SetActive(!d);
    }
}
