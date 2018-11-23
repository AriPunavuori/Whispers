using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DemoStyleEnd : MonoBehaviour {

    public int round = 0;
    public int chain = 0;

    RoundDataManager rdm;

    public TextMeshProUGUI guessText;

    HostGame hg;

    GameManager gm;

    PlayerManager pm;

    UIManager um;

    void Start() {
        rdm = FindObjectOfType<RoundDataManager>();
        hg = FindObjectOfType<HostGame>();
        gm = FindObjectOfType<GameManager>();
        pm = FindObjectOfType<PlayerManager>();
        um = FindObjectOfType<UIManager>();
        um.PocketReset();
    }


    public void Next() {
        um.PocketReset();
        
        if (round >= hg.numberOfPlayers) {
            chain++;
            round = 0;
        }
        if (chain >= hg.numberOfPlayers) {
            chain = 0;
        }
        var ch = rdm.chains[chain];
        if (round % 2 == 0) {
            if (ch.guesses.Count > round) {
                if (round == 0) {
                    guessText.text = ch.guesses[0];
                } else {
                    guessText.text = ch.guesses[round / 2];
                }
            }
        } else {
            var pics = rdm.chains[chain].pictures;
            if (round - 1 == 0) {
                um.ShowPicture(pics[0]);
            } else {
                um.ShowPicture(pics[(round - 1) / 2]);
            }
        }
        round++;
    }
}



//    public void Previous() {
//        if (round % 1 == 0) ;

//    }

//}
