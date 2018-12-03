using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {

    public void PlaySFX() {
        Fabric.EventManager.Instance.PostEvent("nameanim");
    }
}
