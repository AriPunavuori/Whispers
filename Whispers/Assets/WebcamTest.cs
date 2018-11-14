using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WebcamTest : MonoBehaviour {

    public WebCamTexture wc;

    public RawImage RI;

    public string deviceName;

    public WebCamDevice[] devices;

    // Use this for initialization
    void Start() {
        camSetup();

        //WebCamTexture webcamTexture = new WebCamTexture();
        //RI.texture = webcamTexture;
        //RI.material.mainTexture = webcamTexture;
        //webcamTexture.Play();

    }

    // For photo varibles

    public Texture2D heightmap;
    public Vector3 size = new Vector3(100, 10, 100);

    public void camSetup() {
        devices = WebCamTexture.devices;
        deviceName = devices[0].name;
        wc = new WebCamTexture(deviceName, 300, 580, 60);
        RI.texture = wc;
        RI.material.mainTexture = wc;
        wc.Play();
    }

    void OnGUI() {
        if (GUI.Button(new Rect(10, 70, 50, 30), "Click")) {
            TakeSnapshot();
        }

        if (GUI.Button(new Rect(100, 70, 50, 30), "Cam")) {
            ChangeCam();
        }

    }

    // For saving to the _savepath
    private string _SavePath = "C:/Users/oskari.hermunen/Desktop/WCPICS"; //Change the path here!
    int _CaptureCounter = 0;

    void ChangeCam() {
        
    }

    void TakeSnapshot() {
        Texture2D snap = new Texture2D(wc.width, wc.height);
        snap.SetPixels(wc.GetPixels());
        snap.Apply();

        System.IO.File.WriteAllBytes(_SavePath + _CaptureCounter.ToString() + ".png", snap.EncodeToPNG());
        ++_CaptureCounter;
    }
}