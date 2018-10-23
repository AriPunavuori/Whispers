using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMover : MonoBehaviour {

    Canvas canvas;

    void Start() {
        canvas = FindObjectOfType<Canvas>();
        transform.parent = canvas.transform;
    }
	
	void Update () {
		
	}
}
