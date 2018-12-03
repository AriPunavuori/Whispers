using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ToggleGameObject : MonoBehaviour {

    public GameObject NameSetContainer;
    

    public void ShowCont() {
        NameSetContainer.SetActive(true);
    }
}
