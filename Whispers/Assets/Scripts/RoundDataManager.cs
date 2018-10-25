using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundDataManager : MonoBehaviour {

    [System.Serializable]
    public struct RoundData {
        // Kierroksen aikana näihin tulee piirustus- ja kirjotusdatat pelaajilta.
        public LineData lineData;
        public List<string> textData;
}


	void Start () {
		
	}
	
	void Update () {
		
	}
}
