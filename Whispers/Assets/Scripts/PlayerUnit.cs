using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// A PlayerUnit is a unit controlled by a player

public class PlayerUnit : NetworkBehaviour {

    Vector2 velocity;

    // The position we think is most correct for this player.
    // NOTE: if we are authority, then his will be exactly
    // the same transform.position.
    Vector2 bestGuessPosition;

    // This is contantly updated value about our latency to the server
    // ie. how many seconds it takes for us to receive one-way message.
    // (We get it probably from the PlayerConnectionObject)
    float ourLatency;

    float latencySmoothingFactor = 5;

	void Update () {
        // This update runs ALL playerUnits, not just the one I own.
        // How do I verify that I am allowed to mess around with this object?
        if (hasAuthority == false) {

            // We aren't the authority for this object, but we still need to update
            // our local position for this object based on our best guess of
            // where it probably is on owning players screen.

            bestGuessPosition = bestGuessPosition + (velocity * Time.deltaTime);

            // Instead of teleporting our position to best guess's position, we
            // can smoothly lerp to it

            transform.position = Vector2.Lerp(transform.position, bestGuessPosition, Time.deltaTime * latencySmoothingFactor);

            return;
        }

        // if we go here, we are authoratative owner of this object
        transform.Translate(velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.Space)) {
            this.transform.Translate(0, 1, 0);
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            Destroy(gameObject);
        }

        if (Input.GetKeyDown(KeyCode.V)) {
            velocity = new Vector2(1, 0);
            CmdUpdateVelocity(velocity, transform.position);
        }
    }

    [Command]
    void CmdUpdateVelocity(Vector2 v, Vector2 p) {
        // I am on a server.
        transform.position = p;
        velocity = v;

        // If we know what our current latency is, we could do something like this:
        // transform.position = p + (v * (thisPlayerLatencyToServer))

        // Now let the clients know the correct position of this object.
        RpcUpdateVelocity(velocity, transform.position);
    }

    [ClientRpc]
    void RpcUpdateVelocity(Vector2 v, Vector2 p) {
        // I am on a client.

        if (hasAuthority) {
            // This is my own object. I "should" already have the most accurate
            // position/velocity (possibly even more accurate) than the server
            // depending on the game, I might want to change to patch this info
            // from the server, even thought that might look little wonky to the user.

            // Lets assume for now we're just going to ignore the message from the server.
            return;
        }

        // Im non-authoratative client, so I definitely need to listen the server.

        // If we know what our current latency is, we could do something like this:
        // transform.position = p + (v * (ourLatency))

        // transform.position = p;

        velocity = v;
        bestGuessPosition = p + (v * (ourLatency));
 



        // Now player one position is as close as possible on all player's screens.

        // IN FACT, we dont want directly update transform.position, because then
        // players will keep teleporting/blinking as the update comes in. Thats no good.


    }
}
