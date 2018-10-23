using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingMachine : MonoBehaviour {
    public static DrawingMachine instance;
    public List<List<Vector3>> lines;

    private void Awake() {
        lines = new List<List<Vector3>>();
        instance = this;
    }

    void Update () {
        if(InputManager.instance.IsDrawing()){

        }
	}



}
