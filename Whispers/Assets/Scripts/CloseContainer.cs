using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseContainer : MonoBehaviour {

	public void CloseCont() {
        FindObjectOfType<PlayerManager>().textContainer.SetActive(false);
    }
}
