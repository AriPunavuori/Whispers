using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScript : MonoBehaviour {

    public Texture2D RTImage () {
        var camera = GetComponent<Camera>();
        var currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;
        camera.Render();
        Texture2D Image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        Image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        Image.Apply();
        RenderTexture.active = currentRT;
        return Image;
    }
}
