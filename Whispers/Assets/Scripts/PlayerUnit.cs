using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// A PlayerUnit is a unit controlled by a player

public class PlayerUnit : NetworkBehaviour {

	void Update () {
        // This update runs ALL playerUnits, not just the one I own.
        // How do I verify that I am allowed to mess around with this object?
        if (hasAuthority == false) {
            return;
        }
        if (Input.GetKey(KeyCode.Space)) {
            this.transform.Translate(0, 1, 0);
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            Destroy(gameObject);
        }
    }
}
