using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour {

    RoundDataManager rdm;

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
        while (true) {
            if (ch.guesses.Count <= round)
                break;
            // do something - ch.guesses[round]
            //ch.guesses[0];
            round++;
            if (ch.pictures.Count <= round)
                break;
            // do something - ch.pictures[round]
            //ch.pictures[1];
            round++;
        }

    }


}
