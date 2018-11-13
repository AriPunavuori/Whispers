using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WebcamTest : MonoBehaviour {

    public WebCamTexture wc;

    public RawImage RI;

	// Use this for initialization
	void Start () { 

    //WebCamDevice[] devices = WebCamTexture.devices;
    //    for (int i=0;i <= devices.Length; i++) {
    //        Debug.Log(devices[i].name);
    //    }

        WebCamTexture webcamTexture = new WebCamTexture();
        RI.texture = webcamTexture;
        RI.material.mainTexture = webcamTexture;
        webcamTexture.Play();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
