using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGameObject : MonoBehaviour {

    public GameObject NameSetContainer;

    public void ShowCont() {
        print("testi");
        NameSetContainer.SetActive(true);
    }
    
    public void HideCont() {
        NameSetContainer.SetActive(false);
    }

}
