using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;

public class Watching : MonoBehaviour {

    int round = -1;
    public int chain = 0;
    public GameObject nextButton;
    public GameObject previousButton;
    public GameObject quitButton;


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
        um.PocketReset();
    }


    public void Next() {
        um.PocketReset();
        round++;
        //guessText.text = "";
        if(round>0){
            previousButton.gameObject.SetActive(true);
        }

        if (round >= hg.numberOfPlayers) {

            if(chain + 1 >= hg.numberOfPlayers && round + 1 >= hg.numberOfPlayers){
                nextButton.gameObject.SetActive(false);
                var pco = FindObjectOfType<PlayerConnectionObject>();
                pco.CmdReadyToQuit();
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
            var pics = rdm.chains[chain].pictures;
            if (round - 1 == 0) {
                uiText.text = "to which this " + pm.playerDataList[((chain - 1 + hg.numberOfPlayers) % hg.numberOfPlayers)].playerName + " drew as";
                um.ShowPicture(pics[0]);
            } else {
                uiText.text = "which " + pm.playerDataList[(chain - round + hg.numberOfPlayers) % hg.numberOfPlayers].playerName + " drew as";
                um.ShowPicture(pics[(round - 1) / 2]);
            }
        }
    }

    public void Previous() {
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
        Destroy(GameObject.Find("PlayerManager"));
        //Destroy(GameObject.Find("NetworkManager"));
        nm.StopHost();
        //nm.StopMatchMaker();
        //NetworkManager.Shutdown();
        //SceneManager.UnloadSceneAsync(0);
        //Destroy(nm);

        StartCoroutine(WaitKill());
        //SceneManager.LoadScene(0);
    }

    IEnumerator WaitKill(){
        print("Time: " + Time.time);
        yield return new WaitForSeconds(4);
        LoadScene();
        print("Time: " + Time.time);

    }

    void LoadScene(){
        SceneManager.LoadScene(0);
    }

}
