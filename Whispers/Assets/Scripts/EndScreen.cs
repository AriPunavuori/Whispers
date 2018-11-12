using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour {

    public Camera eCam;

    public GameObject picPrefab;
    public Transform origPic;

    RoundDataManager rdm;

    public Text GuessText;

    public Vector3 picPos;

	// Use this for initialization
	void Start () {
        rdm = GameObject.FindObjectOfType<RoundDataManager>();
	}
	

    void ThreadDisplay() {
        //foreach (var data in rdm.chains) {
        //    foreach (var pic in 
        //}
        var ch = rdm.chains[0];
        int round = 0;
        picPos = new Vector3(origPic.position.x, origPic.position.y + (-650 * round), origPic.position.z);
        while (true) {
            if (ch.guesses.Count <= round)
                break;
            Instantiate(picPrefab, picPos, Quaternion.identity, origPic);
            // do something - ch.guesses[round]
            //SMTalk.text = ch.guesses[round];
            round++;
            if (ch.pictures.Count <= round)
                break;
            Instantiate(picPrefab, picPos, Quaternion.identity, origPic);

            // do something - ch.pictures[round]

            //ch.pictures[1];
            round++;
        }

    }


}
