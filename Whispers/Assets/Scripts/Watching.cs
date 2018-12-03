using System.Collections;
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

    RoundDataManager rdm;

    public TextMeshProUGUI uiText;

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
        artNoun = aNoun.text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        um.PocketReset();
    }


    public void Next() {
        um.PocketReset();
        round++;
        Fabric.EventManager.Instance.PostEvent("next");
        //guessText.text = "";
        if(round>0){
            previousButton.gameObject.SetActive(true);
        }

        if (round >= hg.numberOfPlayers) {

            if(chain + 1 >= hg.numberOfPlayers && round + 1 >= hg.numberOfPlayers){
                nextButton.gameObject.SetActive(false);
                var pco = GameObject.Find("" + pm.playerData.playerID).GetComponent<PlayerConnectionObject>();
                if(!readyToQuit){
                    pco.CmdReadyToQuit();
                    readyToQuit = true;
                }
            } else{
                chain++;
                round = 0;
                uiText.text = "Next chain of events looks like this:";
            }
        }

        var ch = rdm.chains[chain];

        if (round % 2 == 0) {
            if (round == 0) {
                uiText.text = pm.playerDataList[((chain - 1 + hg.numberOfPlayers) % hg.numberOfPlayers)].playerName + " was asked to draw " + ch.guesses[0];
            } else {
                uiText.text = "Which " + pm.playerDataList[(chain - round + hg.numberOfPlayers) % hg.numberOfPlayers].playerName + " deciphered as:\n " + ch.guesses[round / 2];
            }
               // (chain % hg.numberOfPlayers - 1)


        } else {
            var aNoun = artNoun[Random.Range(0, artNoun.Length)];
            var pics = rdm.chains[chain].pictures;
            if (round - 1 == 0) {
                uiText.text = "which this " + aNoun/*pm.playerDataList[((chain - 1 + hg.numberOfPlayers) % hg.numberOfPlayers)].playerName*/ + " drew as";
                um.ShowPicture(pics[0]);
            } else {
                uiText.text = "which " + pm.playerDataList[(chain - round + hg.numberOfPlayers) % hg.numberOfPlayers].playerName + " drew as";
                um.ShowPicture(pics[(round - 1) / 2]);
            }
        }
    }

    public void Previous() {
        Fabric.EventManager.Instance.PostEvent("prev");
        round--;
        um.PocketReset();
        if(round<hg.numberOfPlayers){
            nextButton.gameObject.SetActive(true);
        }
        //guessText.text = "";

        if(round == 0 && chain == 0){
            previousButton.gameObject.SetActive(false);
        }

        if(round < 0) {
            chain--;
            round = hg.numberOfPlayers;
            //if(chain == 0 ) {
            //    chain = hg.numberOfPlayers - 1; //Teksti thats it folks ja nappi play again?
            //}
        }

        var ch = rdm.chains[chain];

        if(round % 2 == 0) {

            if(round == 0) {
                uiText.text = pm.playerDataList[((chain - 1 + hg.numberOfPlayers) % hg.numberOfPlayers)].playerName + " was asked to draw " + ch.guesses[0];
            } else {
                uiText.text = "Which " + pm.playerDataList[(chain - round + hg.numberOfPlayers) % hg.numberOfPlayers].playerName + " deciphered as:\n " + ch.guesses[round / 2];
            }
            // (chain % hg.numberOfPlayers - 1)


        } else {
            var pics = rdm.chains[chain].pictures;
            if(round - 1 == 0) {
                uiText.text = "to which this " + pm.playerDataList[((chain - 1 + hg.numberOfPlayers) % hg.numberOfPlayers)].playerName + " drew as";
                um.ShowPicture(pics[0]);
            } else {
                uiText.text = "which " + pm.playerDataList[(chain - round + hg.numberOfPlayers) % hg.numberOfPlayers].playerName + " drew as";
                um.ShowPicture(pics[(round - 1) / 2]);
            }
        }
    }

    public void QuitGame(){
        var nm = FindObjectOfType<NetworkManager>();
        //var pm = FindObjectOfType<PlayerManager>();
        Fabric.EventManager.Instance.PostEvent("stop");
        Fabric.EventManager.Instance.PostEvent("stopmenu");
        Fabric.EventManager.Instance.PostEvent("stoprun");
        Fabric.EventManager.Instance.PostEvent("swish");


        MatchInfo matchInfo = nm.matchInfo;

        Destroy(GameObject.Find("PlayerManager"));
        Destroy(GameObject.Find("Audio Manager"));

        nm.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, nm.OnDropConnection);
        nm.StopHost();
        Destroy(GameObject.Find("NetworkManager"));
        StartCoroutine(WaitKill(1));
        //SceneManager.LoadScene(0);

        //nm.StopMatchMaker();
        //NetworkManager.Shutdown();
        //SceneManager.UnloadSceneAsync(0);
        //StartCoroutine(WaitKill());
    }

    IEnumerator WaitKill(float t){
        yield return new WaitForSeconds(t);
        LoadScene();

    }

    void LoadScene(){
        SceneManager.LoadScene(0);
    }

}
