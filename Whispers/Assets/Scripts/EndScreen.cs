﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour {

    //public CamScript eCam;
    public GameObject picPrefab;
    //public RenderTexture camImage;
    public Transform origPic;
    public Vector3 picPos;
    public GameObject LinePrefab;
    public GameObject pocket;

    public Texture2D TestImage;


    RoundDataManager rdm;

    public int round = 0;


    // Use this for initialization
    void Start() {
        rdm = FindObjectOfType<RoundDataManager>();
        //ThreadDisplay();
    }


    public void ThreadDisplay() {
        rdm = FindObjectOfType<RoundDataManager>();
        var ch = rdm.chains[0];
        //int round = 0;
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
                //var pacImage = pac.GetComponent<RawImage>().texture;
                //pacImage = camImage;
                foreach (var l in ch.pictures[round]) {
                    var drawnLine = Instantiate(LinePrefab);
                    //drawnLine.transform.parent = eCam.transform;
                    var lineToDraw = drawnLine.GetComponent<LineRenderer>();
                    lineToDraw.positionCount = l.points.Length;
                    lineToDraw.SetPositions(l.points);
                }
                //var tex = eCam.RTImage();
                //var spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                //pac.GetComponent<Image>().sprite = spr;
                round++;
                // do something - ch.pictures[round]

            } else {
                break;
            }
        }
    }
}
