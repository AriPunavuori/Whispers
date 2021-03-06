﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class Watching : MonoBehaviour {

    int round = -1;
    public int chain = 0;
    public GameObject nextButton;
    public GameObject previousButton;
    public GameObject quitButton;
    bool readyToQuit;

    string[] artNoun;
    string[] separators = new string[] { "\r\n", "\n" };

    public TextAsset aNoun;
    public TextMeshProUGUI uiText;
    public TextMeshProUGUI chainNbrText;

    HostGame hg;
    GameManager gm;
    PlayerManager pm;
    UIManager um;
    RoundDataManager rdm;
    UnityAdsExample UAE;

    void Start() {
        UAE = FindObjectOfType<UnityAdsExample>();
        rdm = FindObjectOfType<RoundDataManager>();
        hg = FindObjectOfType<HostGame>();
        gm = FindObjectOfType<GameManager>();
        pm = FindObjectOfType<PlayerManager>();
        um = FindObjectOfType<UIManager>();
        artNoun = aNoun.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        um.PocketReset();
    }


    public void Next() {
        Fabric.EventManager.Instance.PostEvent("next");

        um.PocketReset();

        round++;

       

        if (round>0){
            previousButton.gameObject.SetActive(true);
        }

        if(chain + 2 > hg.numberOfPlayers && round + 1 > hg.numberOfPlayers) {
            nextButton.gameObject.SetActive(false);
            //um.uiText.text = "Thats it folks, move along!";
            var pco = GameObject.Find("" + pm.playerData.playerID).GetComponent<PlayerConnectionObject>();
            if(!readyToQuit) {
                pco.CmdReadyToQuit();
                readyToQuit = true;
            }
        } else if(round > hg.numberOfPlayers){
            chain++;
            round = 0;
           

        }

        var ch = rdm.chains[chain];

        if(round % 2 == 0) {
            if(round == 0) {
                uiText.text = pm.playerDataList[((chain - 1 + hg.numberOfPlayers) % hg.numberOfPlayers)].playerName + " was asked to draw " + ch.guesses[0];
            } else {
                uiText.text = "Which " + pm.playerDataList[(chain - round + hg.numberOfPlayers) % hg.numberOfPlayers].playerName + " deciphered as:\n " + ch.guesses[round / 2];
            }
        } else {
            var aNoun = artNoun[Random.Range(0, artNoun.Length)];
            var pics = rdm.chains[chain].pictures;
            if(round == 1) {
                uiText.text = "which this " + aNoun/*pm.playerDataList[((chain - 1 + hg.numberOfPlayers) % hg.numberOfPlayers)].playerName*/ + " drew as";
                um.ShowPicture(pics[0]);
            } else {
                uiText.text = "which " + pm.playerDataList[(chain - round + hg.numberOfPlayers) % hg.numberOfPlayers].playerName + " drew as";
                um.ShowPicture(pics[(round - 1) / 2]);
            }
        }
        chainNbrText.text = "Chain " + (chain + 1) + "/" + hg.numberOfPlayers;
    }

    public void Previous() {

        Fabric.EventManager.Instance.PostEvent("prev");

        um.PocketReset();

        round--;

        if(round<hg.numberOfPlayers){
            nextButton.gameObject.SetActive(true);
        }

        if(round == 0 && chain == 0){
            previousButton.gameObject.SetActive(false);
        }

        if(round < 0) {
            chain--;
            round = hg.numberOfPlayers;
        }

        var ch = rdm.chains[chain];

        if(round % 2 == 0) {

            if(round == 0) {
                uiText.text = pm.playerDataList[((chain - 1 + hg.numberOfPlayers) % hg.numberOfPlayers)].playerName + " was asked to draw " + ch.guesses[0];
            } else {
                uiText.text = "Which " + pm.playerDataList[(chain - round + hg.numberOfPlayers) % hg.numberOfPlayers].playerName + " deciphered as:\n " + ch.guesses[round / 2];
            }

        } else {

            var pics = rdm.chains[chain].pictures;
            if(round == 1) {
                uiText.text = "to which this " + pm.playerDataList[((chain - 1 + hg.numberOfPlayers) % hg.numberOfPlayers)].playerName + " drew as";
                um.ShowPicture(pics[0]);
            } else {
                uiText.text = "which " + pm.playerDataList[(chain - round + hg.numberOfPlayers) % hg.numberOfPlayers].playerName + " drew as";
                um.ShowPicture(pics[(round - 1) / 2]);
            }
        }
        chainNbrText.text = "Chain " + (chain + 1) + "/" + hg.numberOfPlayers;
    }

    public void QuitGame(){
        var nm = FindObjectOfType<NetworkManager>();
        var pco = FindObjectOfType<PlayerConnectionObject>();
        //var pm = FindObjectOfType<PlayerManager>();
        Fabric.EventManager.Instance.PostEvent("stop");
        Fabric.EventManager.Instance.PostEvent("stopmenu");
        Fabric.EventManager.Instance.PostEvent("stoprun");
        Fabric.EventManager.Instance.PostEvent("swish");

        pco.CmdQuit();
        //MatchInfo matchInfo = nm.matchInfo;



        //nm.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, nm.OnDropConnection);
        //Destroy(GameObject.Find("NetworkManager"));
        //StartCoroutine(WaitKill(1));

        //--------------------------------------

        //SceneManager.LoadScene(0);

        //nm.StopMatchMaker();
        //NetworkManager.Shutdown();
        //SceneManager.UnloadSceneAsync(0);
        //StartCoroutine(WaitKill());
    }

}
