using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour {

    public Camera eCam;
    public GameObject picPrefab;
    public RenderTexture camImage;
    public Text GuessText;
    public Transform origPic;
    public Vector3 picPos;

    RoundDataManager rdm;


    // Use this for initialization
    void Start() {
        rdm = GameObject.FindObjectOfType<RoundDataManager>();
    }


    void ThreadDisplay() {
        var ch = rdm.chains[0];
        int round = 0;
        picPos = new Vector3(origPic.position.x, (origPic.position.y - (650 * round)), origPic.position.z);
        while (true) {
            if (ch.guesses.Count <= round) {
                var pic = Instantiate(picPrefab, picPos, Quaternion.identity, origPic);
                var picText = pic.GetComponentInChildren<Text>();
                picText.text = ch.guesses[round];
                round++;
            } else {
                break;
            }

            if (ch.pictures.Count <= round) {
                var pac = Instantiate(picPrefab, picPos, Quaternion.identity, origPic);
                var pacImage = pac.GetComponent<RawImage>().texture;
                pacImage = camImage;
                round++;
                // do something - ch.pictures[round]
            } else {
                break;
            }
        }

    }


}
